using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellController : MonoBehaviour
{
	private static CellController _instance;
	public static CellController Instance { get { return _instance = _instance ?? FindObjectOfType<CellController>(); } }

	public static event Action CellChanged;

	public CellController()
	{
	}

	public GroundCell[] cells;
	public Stage[] CurrentGameCellsPlace { get; private set; }

	private void Awake()
	{
		SubscribeOnCells();
	}


	private void SubscribeOnCells()
	{
		cells.ToList().ForEach(cell => cell.Click += OnCellClick);
	}

	private void OnCellClick(int cellNumber)
	{
		cells[cellNumber].ChangeCell(UserController.Instance.currentUser.UserStage);
		CurrentGameCellsPlace[cellNumber] = UserController.Instance.currentUser.UserStage;

		if (CheckWinningCombination()) return;
		var handler = CellChanged;
		if (handler != null) handler();
		else Debug.Log("Action  CellChanged is null at :" + this);

	}

	private bool CheckIfFreeCellsExist()
	{
		return CurrentGameCellsPlace.Any(cellStage => cellStage == Stage.NAN);
	}


	public void RefreshCellsData()
	{
		CurrentGameCellsPlace = new Stage[9];
		cells.ToList().ForEach(cell => cell.RefreshCell());
	}

	private bool CheckWinningCombination()
	{
		if (CheckWinnerByState(Stage.CROSS)) return true;

		if (CheckWinnerByState(Stage.ZERO)) return true;

		if (!CheckIfFreeCellsExist())
		{
			WindowManager.Instance.GetWindow<WinWindowController>(new Hashtable { { "name", "Draw" } });
			return true;
		}
		return false;
	}

	private bool CheckWinnerByState(Stage stage)
	{
		if (IsWinning(stage))
		{
			WindowManager.Instance.GetWindow<WinWindowController>(new Hashtable { { "name", UserController.Instance.GetUserNameByStage(stage) } });
			return true;
		}
		return false;
	}



	private bool IsWinning(Stage stage)
	{
		Stage[] neededStages = new Stage[CurrentGameCellsPlace.Length];
		for (int i = 0; i < CurrentGameCellsPlace.Length; i++)
		{
			if (CurrentGameCellsPlace[i] == stage) neededStages[i] = CurrentGameCellsPlace[i];
			else neededStages[i] = Stage.NAN;
		}

		var variationsSO = ScriptableObjectManager.Instance.GetObject<GameVariationsScrptblObject>();

		var variationsArr = variationsSO.Variations;

		for (int i = 0; i < variationsArr.Length; i++)
		{
			var currVariation = variationsArr[i].variation;
			var winningCount = 0;
			for (int j = 0; j < currVariation.Length; j++)
			{
				if (currVariation[j] == false) continue;
				if (currVariation[j] == true && neededStages[j] != stage) break;
				winningCount++;
			}
			if (winningCount == 3) return true;
		}
		return false;
	}
}
