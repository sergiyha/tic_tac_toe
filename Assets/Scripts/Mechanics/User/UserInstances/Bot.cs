using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot : AbstractUser
{
	public Bot(Stage stage, string name) : base(stage, name)
	{
	}

	public override void Play()
	{
		base.Play();

		int cellToPlace = ChooseOptimalVariation();
		CellController.Instance.cells[cellToPlace].SetButton.onClick.Invoke();
		//Debug.Log(cellToPlace);
		//Debug.Log("But i'm a BOT");
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
		var variation = ScriptableObjectManager.Instance.GetObject<GameVariationsScrptblObject>().Variations;
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
			var variation = ScriptableObjectManager.Instance.GetObject<GameVariationsScrptblObject>().Variations[id].variation;
			for (int i = 0; i < variation.Length; i++)
			{
				if (variation[i] == true && gameCellPosition[i] == Stage.NAN)
				{
					positionsCanPlace.Add(i);
					//neededCellPosition = i;
					//break;
				}
			}

			neededCellPosition = positionsCanPlace[Random.Range(0, positionsCanPlace.Count)];
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

		var variations = ScriptableObjectManager.Instance.GetObject<GameVariationsScrptblObject>().Variations;
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
			variationId = bestVariationsId[Random.Range(0, bestVariationsId.Count)];
			//variationId = variationId_CoincidenceCount.First(id_Count => id_Count.Value == variationId_CoincidenceCount.Values.Max()).Key;
		}
		return variationId;
	}

	private int GetFreePosition()//call when useful variations == 0
	{
		var gameCellsPlace = CellController.Instance.CurrentGameCellsPlace;
		int freeCell = 0;
		for (int i = 0; i < gameCellsPlace.Length; i++)
		{
			if (gameCellsPlace[i] == Stage.NAN)
			{
				freeCell = i;
			}

		}
		return freeCell;
	}


	private Dictionary<int, Variation> GetVariationsThatCanBeUsefullAtFirst()
	{
		var usefulVariations = new Dictionary<int, Variation>();
		var gameCellsPlace = CellController.Instance.CurrentGameCellsPlace;
		var variations = ScriptableObjectManager.Instance.GetObject<GameVariationsScrptblObject>().Variations;
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

		var neededCell = listOfWithMaxPriority[Random.Range(0, listOfWithMaxPriority.Count)];
		//	var neededCell = cellNumberPriority.First(cellpriority => cellpriority.Value == cellNumberPriority.Values.Max()).Key;
		return neededCell;
	}
}

