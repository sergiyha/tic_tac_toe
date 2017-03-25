using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Window;

public class StartWindowController : Window
{
	public StartWindowView StartWindowView;

	private void Awake()
	{
		StartWindowView.ClickOneVsBot += OnOneVsBotClick;
		StartWindowView.ClickOneVsOne += OnOneVsOneClick;
		StartWindowView.ClickBotVsOne += OnBotVsOneClick;
	}


	private void OnBotVsOneClick()
	{
		GameManager.Instance.StartGame(GameType.BotVsOne);
	}

	private void OnOneVsOneClick()
	{
		GameManager.Instance.StartGame(GameType.OneVsOne);
	}

	private void OnOneVsBotClick()
	{
		GameManager.Instance.StartGame(GameType.OneVsBot);
	}

	public override void OpenWindow(Hashtable table = null)
	{
		base.OpenWindow();
	}

	public override void CloseWindow()
	{
		base.CloseWindow();
	}


}
