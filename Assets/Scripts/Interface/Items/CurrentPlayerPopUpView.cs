using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerPopUpView : MonoBehaviour
{
	public Text CurrentPlayer;

	public void InitItem(string currentPlayer)
	{
		CurrentPlayer.text = "Current: " + currentPlayer;
	}
}
