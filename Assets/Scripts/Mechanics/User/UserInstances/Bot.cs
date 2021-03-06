﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot : AbstractUser
{
	private GameDifficulty _botDifficulty;
	private GameVariationsScrptblObject _gameVariationsSO;
	private ProbabilityScriptableObject _probabilitySo;
	private bool[] _probabilityToMistake;

	public Bot(Stage stage, string name, GameDifficulty difficulty) : base(stage, name)
	{
		_botDifficulty = difficulty;
		_gameVariationsSO = ScriptableObjectManager.Instance.GetObject<GameVariationsScrptblObject>();
		_probabilitySo = ScriptableObjectManager.Instance.GetObject<ProbabilityScriptableObject>();
		_probabilityToMistake = ChooseProbability();
	}

	private bool ShouldBotMakeMistake()
	{
		return _probabilityToMistake[UnityEngine.Random.Range(0, _probabilityToMistake.Length)];
	}

	private bool[] ChooseProbability()
	{
		var neededProbabilityInstance = _probabilitySo.ProbabilityInnstances.First(pI => pI.Difficulty == _botDifficulty);
		return neededProbabilityInstance.Probability;
	}

	public override void Play()
	{
		base.Play();

		int cellToPlace = GetCellToPlaceWithMistake();
		CellController.Instance.cells[cellToPlace].SetButton.onClick.Invoke();
	}

	private int GetCellToPlaceWithMistake()
	{
		if (ShouldBotMakeMistake())//сделал ли бот ошибку
		{
			var listOfEnemyWinningCombinations = CheckIfUserHaveWinningCombination(_enemyStage, UserStage);
			var listOfUserWinningCombinations = CheckIfUserHaveWinningCombination(UserStage, _enemyStage);
			if (listOfUserWinningCombinations.Count >= 1)//может ли бот выиграть 
			{
				return GetProtectedStep(listOfUserWinningCombinations);
			}
			else if (listOfEnemyWinningCombinations.Count >= 1 && _botDifficulty == GameDifficulty.Normal)//если средний бот то поверяет может ли выиграть соперник
			{
				if (!ShouldBotMakeMistake())//ошибается ли бот при попытке помешать сопернику
				{
					return GetProtectedStep(listOfEnemyWinningCombinations);//если нет то не дает выиграть 
				}
				else
				{
					return GetFreePosition();
				}
			}
			else//если не может выиграть или помешать то тупит
			{
				return GetFreePosition();//занимает рандомную пустую 
			}
		}
		else//если бот не делает ошибку то отрабатывает стандартная логика 
		{
			return ChooseOptimalVariation();
		}
	}

	private Stage _enemyStage { get { return UserController.Instance.users.SingleOrDefault(user => user.GetUserStage() != UserStage).GetUserStage(); } }

	private int ChooseOptimalVariation()
	{
		if (!CellController.Instance.CurrentGameCellsPlace.Any(stage => stage == UserStage))//if there is no one bot stage on field
			return GetOptimalPlaceOfChosenVariationsAtFirst(GetVariationsThatCanBeUsefullAtFirst());
		else
		{
			var listOfEnemyWinningCombinations = CheckIfUserHaveWinningCombination(_enemyStage, UserStage);
			if (listOfEnemyWinningCombinations.Count >= 1)
			{
				var listOfUserWinningCombinations = CheckIfUserHaveWinningCombination(UserStage, _enemyStage);
				if (listOfUserWinningCombinations.Count >= 1)
					return GetProtectedStep(listOfUserWinningCombinations);

				return GetProtectedStep(listOfEnemyWinningCombinations);
			};
			return GetFreePoistionInVariation(GetMostUsefulVariationId());
		}
	}

	private List<Variation> CheckIfUserHaveWinningCombination(Stage checkedUser, Stage oppositeOpponent)
	{
		var userWinningVariations = new List<Variation>();
		var gameCellPosition = CellController.Instance.CurrentGameCellsPlace;
		var variation = _gameVariationsSO.Variations;
		for (int i = 0; i < variation.Length; i++)
		{
			var currVariation = variation[i].variation;
			int userWinningCount = 0;

			for (int j = 0; j < currVariation.Length; j++)
			{
				if (currVariation[j] == false) continue;
				if (gameCellPosition[j] == oppositeOpponent)
				{
					userWinningCount = 0;
					break;
				}
				if (currVariation[j] == true && gameCellPosition[j] == checkedUser)
				{
					userWinningCount++;
				}
			}
			if (userWinningCount == 2) { userWinningVariations.Add(variation[i]); }
		}
		return userWinningVariations;
	}

	private int GetProtectedStep(List<Variation> winningCombinations)
	{
		int protectedCellPosition = 0;
		var gameCellPosition = CellController.Instance.CurrentGameCellsPlace;

		var winningWariation = winningCombinations.First().variation;
		for (int i = 0; i < winningWariation.Length; i++)
		{
			if (winningWariation[i] == false) continue;
			if (winningWariation[i] == true && gameCellPosition[i] == Stage.NAN)
			{
				protectedCellPosition = i;
				break;

			}
		}

		return protectedCellPosition;
	}

	private int GetFreePoistionInVariation(int id)
	{
		int neededCellPosition = 0;
		if (id != -1)
		{
			var gameCellPosition = CellController.Instance.CurrentGameCellsPlace;
			List<int> positionsCanPlace = new List<int>();
			var variation = _gameVariationsSO.Variations[id].variation;
			for (int i = 0; i < variation.Length; i++)
			{
				if (variation[i] == true && gameCellPosition[i] == Stage.NAN)
				{
					positionsCanPlace.Add(i);
				}
			}
			if (_botDifficulty == GameDifficulty.Incredible && CheckCrossEnemyPositions())//если бот сложный и угрожает угловая позиция то выбираем нечетный целл из возможных в этой вариации
			{
				neededCellPosition = GetListOfFreeNotPairIndexPositions().Where(pos => positionsCanPlace.Any(canPlace => canPlace == pos)).First();
			}
			else
			{
				neededCellPosition = positionsCanPlace[UnityEngine.Random.Range(0, positionsCanPlace.Count)];
			}
		}
		else
		{
			neededCellPosition = GetFreePosition();
		}
		return neededCellPosition;
	}

	private int GetMostUsefulVariationId()
	{
		Dictionary<int, int> variationId_CoincidenceCount = new Dictionary<int, int>();

		var variations = _gameVariationsSO.Variations;
		var gameCellsPlace = CellController.Instance.CurrentGameCellsPlace;
		int variationId = 0;
		for (int i = 0; i < variations.Length; i++)
		{
			var currVariation = variations[i].variation;
			int coincidenceCount = 0;
			for (int j = 0; j < currVariation.Length; j++)
			{
				if (currVariation[j] == false) continue;
				if (gameCellsPlace[j] == _enemyStage)
				{
					coincidenceCount = 0;
					break;
				}
				if (currVariation[j] == true && gameCellsPlace[j] == UserStage)
					coincidenceCount++;
			}
			variationId_CoincidenceCount.Add(i, coincidenceCount);
		}

		if (variationId_CoincidenceCount.Values.Max() == 0)
		{
			variationId = -1;//if max is zero
		}
		else
		{
			List<int> bestVariationsId = variationId_CoincidenceCount.Keys.Where(id => variationId_CoincidenceCount[id] == variationId_CoincidenceCount.Values.Max()).ToList();
			if (_botDifficulty == GameDifficulty.Incredible && CheckCrossEnemyPositions())//выбираем нужный id вариации который удовлетворяет наши пустые непарные места(если боту угрожает угловая вариация) 
			{
				variationId = GetIndexOfVariationByFreePositions(GetListOfFreeNotPairIndexPositions());
			}
			else
			{
				variationId = bestVariationsId[UnityEngine.Random.Range(0, bestVariationsId.Count)];
			}
		}
		return variationId;
	}

	private int GetIndexOfVariationByFreePositions(List<int> freeIndexPositions)
	{
		List<Variation> listOfNeededVariations = new List<Variation>();
		int place = freeIndexPositions[UnityEngine.Random.Range(0, freeIndexPositions.Count)];
		listOfNeededVariations = _gameVariationsSO.Variations.Where(v => v.variation[place] == true).ToList();
		var neededVariation = listOfNeededVariations[UnityEngine.Random.Range(0, listOfNeededVariations.Count)];
		int variationIndex = Array.FindIndex(_gameVariationsSO.Variations, v => v == neededVariation);
		return variationIndex;
	}

	private List<int> GetListOfFreeNotPairIndexPositions()
	{
		List<int> freeIndexPositions = new List<int>();
		var cellStages = CellController.Instance.CurrentGameCellsPlace;
		for (int i = 0; i < cellStages.Length; i++)
		{
			if (cellStages[i] == Stage.NAN && i % 2 != 0)
			{
				freeIndexPositions.Add(i);
			}
		}
		return freeIndexPositions;
	}

	private bool CheckCrossEnemyPositions()//угрожает ли сложному боту угловая позиция игрока
	{
		int enemyCount = 0;
		var cellStages = CellController.Instance.CurrentGameCellsPlace;
		foreach (var cellStage in cellStages)
		{
			if (cellStage == _enemyStage)
			{
				enemyCount++;
			}
		}

		if (enemyCount == 2)
		{
			if (cellStages[0] == _enemyStage && cellStages[8] == _enemyStage)
			{
				return true;
			}
			else if (cellStages[2] == _enemyStage && cellStages[6] == _enemyStage)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else return false;
	}

	private int GetFreePosition()//call when useful variations == 0
	{
		List<int> freePositions = new List<int>();
		var gameCellsPlace = CellController.Instance.CurrentGameCellsPlace;
		for (int i = 0; i < gameCellsPlace.Length; i++)
		{
			if (gameCellsPlace[i] == Stage.NAN)
			{
				freePositions.Add(i);
			}
		}
		return freePositions[UnityEngine.Random.Range(0, freePositions.Count)];
	}

	private Dictionary<int, Variation> GetVariationsThatCanBeUsefullAtFirst()
	{
		var usefulVariations = new Dictionary<int, Variation>();
		var gameCellsPlace = CellController.Instance.CurrentGameCellsPlace;
		var variations = _gameVariationsSO.Variations;
		for (int i = 0; i < variations.Length; i++)
		{
			var currVariation = variations[i].variation;
			int neededCount = 0;
			for (int j = 0; j < currVariation.Length; j++)
			{
				if (currVariation[j] == false) continue;
				if (currVariation[j] == true && gameCellsPlace[j] == _enemyStage) break;
				neededCount++;
			}
			if (neededCount == 3) usefulVariations.Add(i, variations[i]);
		}
		return usefulVariations;
	}

	private int GetOptimalPlaceOfChosenVariationsAtFirst(Dictionary<int, Variation> chosenVariations)
	{
		Dictionary<int, int> cellNumberPriority = new Dictionary<int, int>();
		foreach (KeyValuePair<int, Variation> idVsVariattion in chosenVariations)
		{
			int cellCounter = 0;
			foreach (var cell in idVsVariattion.Value.variation)
			{
				if (cell == true)
				{
					if (cellNumberPriority.Keys.Contains(cellCounter))
					{
						cellNumberPriority[cellCounter]++;
					}
					else
					{
						cellNumberPriority.Add(cellCounter, 1);
					}
				}
				cellCounter++;
			}
		}

		List<int> listOfWithMaxPriority = cellNumberPriority.Keys.Where(cell => cellNumberPriority[cell] == cellNumberPriority.Values.Max()).ToList();

		var neededCell = listOfWithMaxPriority[UnityEngine.Random.Range(0, listOfWithMaxPriority.Count)];
		return neededCell;
	}
}

