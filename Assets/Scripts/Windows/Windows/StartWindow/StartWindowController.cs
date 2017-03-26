using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Window;

public class StartWindowController : Window
{
	public StartWindowView StartWindowView;
	private GameType _gameType;
	private GameDifficulty _gameDifficulty;

	private void Awake()
	{
		//подписываем контейнер игровых типов на контроллер
		StartWindowView.GameTypeContainer.ClickOneVsBot += OnChooseGameType;
		StartWindowView.GameTypeContainer.ClickOneVsOne += OnChooseGameType;
		StartWindowView.GameTypeContainer.ClickBotVsOne += OnChooseGameType;

		//подписываем контейнер сложностей на контроллер
		StartWindowView.DiffContainer.ClickEasyBttn += OnChooseDifficulty;
		StartWindowView.DiffContainer.ClickNormalBttn += OnChooseDifficulty;
		StartWindowView.DiffContainer.ClickIncredibleBttn += OnChooseDifficulty;
		StartWindowView.DiffContainer.ClickBack += OnClickBack;
	}


	private void OnChooseGameType(GameType gameType)
	{
		if (gameType == GameType.OneVsOne)
		{
			GameManager.Instance.StartGame(gameType);
			return;
		}

		_gameType = gameType;
		StartWindowView.GameTypeContainer.Close();
		StartWindowView.DiffContainer.Open();
	}

	private void OnChooseDifficulty(GameDifficulty gameDifficulty)
	{
		_gameDifficulty = gameDifficulty;
		GameManager.Instance.StartGame(_gameType, _gameDifficulty);
	}

	private void OnClickBack()
	{
		StartWindowView.GameTypeContainer.Open();
		StartWindowView.DiffContainer.Close();
	}






	//private void OnOneVsOneClick()
	//{
	//	GameManager.Instance.StartGame(GameType.OneVsOne);
	//}

	//private void OnOneVsBotClick()
	//{
	//	GameManager.Instance.StartGame(GameType.OneVsBot);
	//}

	public override void OpenWindow(IInputWindowPatamerer parameter)
	{
		base.OpenWindow(parameter);
	}

	public override void CloseWindow()
	{
		base.CloseWindow();
	}


}
