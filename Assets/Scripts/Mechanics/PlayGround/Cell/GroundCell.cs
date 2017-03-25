using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GroundCell : MonoBehaviour
{
	public Button SetButton;
	public Image Cross;
	public Image Zero;
	public int CellNumber;

	public Action<int> Click;

	private void Awake()
	{
		SetButton.onClick.AddListener(delegate { SetButton.enabled = false; Click(CellNumber); });
		CellController.Instance.TryToAddNewCell(this);
	}


	public void RefreshCell()
	{
		SetButton.enabled = true;
		Cross.gameObject.SetActive(false);
		Zero.gameObject.SetActive(false);
	}

	public void ChangeCell(Stage stage)
	{
		switch (stage)
		{
			case Stage.NAN:
				break;
			case Stage.CROSS:
				Cross.gameObject.SetActive(true);
				break;
			case Stage.ZERO:
				Zero.gameObject.SetActive(true);
				break;
			default:
				break;
		}
	}

	public void SetButtonState()
	{


	}








}
