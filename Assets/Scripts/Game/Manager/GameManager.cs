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
	public Stage CurrentWinnerStage
	{
		get; private set;
	}

	public Action<Stage> RoundEnds;
	public static Action<Stage> RoundBegins;

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
		if (RoundEnds == null)
		{
			RoundEnds += CacheCurrentWinner;
		}

	}

	private void CacheCurrentWinner(Stage winnerStage)
	{
		CurrentWinnerStage = winnerStage;
	}

	public void StartGame(GameType gameType)
	{
		CurrGameType = gameType;
		GamePreparation();

	}

	public void GamePreparation()
	{
		switch (CurrGameType)
		{
			case GameType.OneVsOne:
				UserController.Instance.CreateUsers<User, User>();
				break;
			case GameType.OneVsBot:
				UserController.Instance.CreateUsers<User, Bot>();
				break;
			case GameType.BotVsOne:
				UserController.Instance.CreateUsers<Bot, User>();
				break;
			default:
				break;
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


