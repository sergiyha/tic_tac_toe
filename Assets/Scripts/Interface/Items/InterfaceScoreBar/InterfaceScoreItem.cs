using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScoreItem : MonoBehaviour
{
	public Text UserName;
	public Text UserScore;


	public void InitItem(string userName, string userScore)
	{
		UserName.text = userName;
		UserScore.text = userScore;
	}
}

