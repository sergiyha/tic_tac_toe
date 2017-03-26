using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindowGameDifficultyContainer : Container
{
	public Button EasyBttn;
	public Button NormalBttn;
	public Button IncredibleBttn;

	public Button BackBttn;

	public event Action<GameDifficulty> ClickEasyBttn;
	public event Action<GameDifficulty> ClickNormalBttn;
	public event Action<GameDifficulty> ClickIncredibleBttn;

	public event Action ClickBack;

	private void Awake()
	{
		BackBttn.onClick.AddListener(delegate { ClickBack(); });
		EasyBttn.onClick.AddListener(delegate { ClickEasyBttn(GameDifficulty.Easy); });
		NormalBttn.onClick.AddListener(delegate { ClickNormalBttn(GameDifficulty.Normal); });
		IncredibleBttn.onClick.AddListener(delegate { ClickIncredibleBttn(GameDifficulty.Incredible); });
	}
}
