using System;
using System.Collections;
using System.Collections.Generic;
using UI.Window;
using UnityEngine;
using UnityEngine.UI;

public class WinWindowController : Window
{
	public WinWindowView WindowView;

	private Stage _winnerStage;
	private List<AbstractUser> users;

	private void Awake()
	{
		WindowView.RestartButtonClick += OnRestartButtonClick;
		WindowView.ToMainMenuButtonClick += OnToMainMenuButtonClick;
	}

	private void OnToMainMenuButtonClick()
	{
		SceneLoadManager.Instance.StartLoadScene(ScenesToLoad.PregameScene, () => { });
		CloseWindow();
	}

	private void OnRestartButtonClick()
	{
		GameManager.Instance.PrepareGameForNextRound();
		CloseWindow();
	}

	public override void CloseWindow()
	{
		base.CloseWindow();
	}

	public override void OpenWindow(IInputWindowPatamerer windowParameter)
	{
		var neededParameter = windowParameter.GetOriginWindowParameter<WinWindowInputParameter>();
		_winnerStage = neededParameter.WinnerStage;
		users = neededParameter.Users;
		base.OpenWindow(windowParameter);
		InitWindow();


	}

	private void InitWindow()
	{
		var winnerName = (_winnerStage == Stage.NAN) ? "Draw" : UserController.Instance.GetUserByStage(_winnerStage).GetName();
		WindowView.SetCurrentWinnerLabel(winnerName);
		users.ForEach(user =>
		{
			var playerItem = Instantiate(WindowView.PlayerItemInstance) as PlayerInformationItem;
			playerItem.transform.SetParent(WindowView.ItemsGrid.transform, false);
			playerItem.InitItem(user.GetName(), user.GetScore().ToString());
		});

	}

	private void OnDisable()
	{
		ClearGrid();
	}

	private void ClearGrid()
	{
		foreach (Transform item in WindowView.ItemsGrid.transform)
		{
			Destroy(item.gameObject);
		}
	}

}
