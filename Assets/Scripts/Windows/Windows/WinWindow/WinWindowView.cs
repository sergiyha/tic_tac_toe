using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinWindowView : MonoBehaviour
{
	public event Action RestartButtonClick;
	public event Action ToMainMenuButtonClick;

	public Button RestartButton;
	public Button ToMainMenuButton;

	private void Awake()
	{
		RestartButton.onClick.AddListener(delegate { RestartButtonClick(); });
		ToMainMenuButton.onClick.AddListener(delegate { ToMainMenuButtonClick(); });
	}


}
