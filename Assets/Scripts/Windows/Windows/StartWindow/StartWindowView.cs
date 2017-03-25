using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindowView : MonoBehaviour
{
	public Button OneVsOneBttn;
	public Button OneVsBotBttn;
	public Button BotVsOneBttn;

	public event Action ClickOneVsOne;
	public event Action ClickOneVsBot;
	public event Action ClickBotVsOne;

	private void Awake()
	{
		OneVsOneBttn.onClick.AddListener(delegate { ClickOneVsOne(); });
		OneVsBotBttn.onClick.AddListener(delegate { ClickOneVsBot(); });
		BotVsOneBttn.onClick.AddListener(delegate { ClickBotVsOne(); });
	}
}
