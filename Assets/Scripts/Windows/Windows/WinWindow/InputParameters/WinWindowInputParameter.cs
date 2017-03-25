using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinWindowInputParameter : InputWindowParameter
{
	public Stage WinnerStage { get; private set; }
	public List<AbstractUser> Users { get; private set; }

	public WinWindowInputParameter(List<AbstractUser> users, Stage winnerStage)
	{
		WinnerStage = winnerStage;
		Users = users;
	}


}
