using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformationItem : MonoBehaviour
{
	public Text PlayerName;
	public Text PlayerScore;

	public void InitItem(string palyerName, string playerScore)
	{
		PlayerName.text = palyerName;
		PlayerScore.text = playerScore;
	}
}
