using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	private static GameManager _instance;
	public static GameManager Instance { get { return _instance = _instance ?? FindObjectOfType<GameManager>(); } }
	private GameManager() { }

	public GameType CurrGameType { get; private set; }


	public void StartGame(GameType gameType)
	{
		CurrGameType = gameType;
		GamePreparation();

	}

	public void GamePreparation()
	{
		WindowManager.Instance.CloseCurrentWindow();
		CellController.Instance.RefreshCellsData();
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

	}

}
