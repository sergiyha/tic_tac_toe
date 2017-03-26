using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindowGameTypeContainer : Container
{

	public Button OneVsOneBttn;
	public Button OneVsBotBttn;
	public Button BotVsOneBttn;

	public event Action<GameType> ClickOneVsOne;
	public event Action<GameType> ClickOneVsBot;
	public event Action<GameType> ClickBotVsOne;

	private void Awake()
	{
		OneVsOneBttn.onClick.AddListener(delegate { ClickOneVsOne(GameType.OneVsOne); });
		OneVsBotBttn.onClick.AddListener(delegate { ClickOneVsBot(GameType.OneVsBot); });
		BotVsOneBttn.onClick.AddListener(delegate { ClickBotVsOne(GameType.BotVsOne); });
	}
}
