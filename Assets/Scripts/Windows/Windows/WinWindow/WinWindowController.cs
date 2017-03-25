using System;
using System.Collections;
using System.Collections.Generic;
using UI.Window;
using UnityEngine;
using UnityEngine.UI;

public class WinWindowController : Window
{
	public WinWindowView WindowView;
	public Text WinnerName;

	private void Awake()
	{
		WindowView.RestartButtonClick += OnRestartButtonClick;
		WindowView.ToMainMenuButtonClick += OnToMainMEnuButtonClick;
	}



	private void OnToMainMEnuButtonClick()
	{
		WindowManager.Instance.GetWindow<StartWindowController>();
		CloseWindow();
	}

	private void OnRestartButtonClick()
	{
		GameManager.Instance.GamePreparation();
	}

	public override void CloseWindow()
	{
		base.CloseWindow();
	}

	public override void OpenWindow(Hashtable table = null)
	{
		WinnerName.text = table["name"].ToString();
		base.OpenWindow();
	}

}
