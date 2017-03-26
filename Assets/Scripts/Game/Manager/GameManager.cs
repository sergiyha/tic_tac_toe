using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	private static GameManager _instance;
	public static GameManager Instance { get { return _instance = _instance ?? FindObjectOfType<GameManager>(); } }
	private GameManager() { }

	public GameType CurrGameType { get; private set; }
	public GameDifficulty CurrGameDifficulty { get; private set; }

	public Stage CurrentWinnerStage
	{
		get; private set;
	}

	public Action<Stage> RoundEnds;
	public Action<Stage> RoundBegins;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}

		DontDestroyOnLoad(this);

		if (!StaticTools.HandlerChecker.IsHandlerRegistred(CacheCurrentWinner, RoundEnds))
		{
			RoundEnds += CacheCurrentWinner;
		}
	}

	private void CacheCurrentWinner(Stage winnerStage)
	{
		CurrentWinnerStage = winnerStage;
	}

	public void StartGame(GameType gameType, GameDifficulty gameDifficulty = GameDifficulty.NaN)
	{
		CurrGameType = gameType;
		CurrGameDifficulty = gameDifficulty;

		GamePreparation();

	}

	public void GamePreparation()
	{
		if (CurrGameType == GameType.OneVsOne && CurrGameDifficulty == GameDifficulty.NaN)
		{
			UserController.Instance.CreateUserVsUser();
		}
		else if (CurrGameType == GameType.OneVsBot && CurrGameDifficulty != GameDifficulty.NaN)
		{
			UserController.Instance.CreateUserVsBot(CurrGameDifficulty);
		}
		else if (CurrGameType == GameType.BotVsOne && CurrGameDifficulty != GameDifficulty.NaN)
		{
			UserController.Instance.CreateBotVsUser(CurrGameDifficulty);
		}

		SceneLoadManager.Instance.StartLoadScene(ScenesToLoad.FirstScene, _firstSceneCallback);

	}

	public void PrepareGameForNextRound()
	{
		RoundBegins(CurrentWinnerStage);
		CellController.Instance.RefreshCellsData();
		UserController.Instance.ChooseCurrentPlayer();
	}

	private void _firstSceneCallback()
	{
		CellController.Instance.RefreshCellsData();
		UserController.Instance.ChooseCurrentPlayer();
	}
}


