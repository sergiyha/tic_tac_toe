using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScoreBarView : MonoBehaviour
{
	public GridLayoutGroup ItemsGrid;
	public Button BackButton;

	public event Action Back;


	private void Awake()
	{
		BackButton.onClick.AddListener(delegate { Back(); });
	}


}
