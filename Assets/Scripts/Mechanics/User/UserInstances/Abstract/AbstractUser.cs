using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractUser
{
	protected AbstractUser(Stage stage, string name)
	{
		UserStage = stage;
		Name = name;
		GameManager.Instance.RoundEnds += OnRoundEnds;
		GameManager.Instance.RoundBegins += OnRoundBegins;
	}


	protected Stage UserStage;
	protected string Name;
	protected int Score;

	public virtual void Play()
	{
		//Debug.Log("User with stage: " + UserStage + " is current");
	}

	public string GetName()
	{
		return this.Name;
	}

	public Stage GetUserStage()
	{
		return UserStage;
	}

	public int GetScore()
	{
		return Score;
	}

	private void OnRoundBegins(Stage winnerStage)
	{
		if (winnerStage == UserStage)
		{
			UserStage = Stage.CROSS;
		}
		else if (winnerStage == Stage.NAN)
		{
			UserStage = (UserStage == Stage.CROSS) ? Stage.ZERO : Stage.CROSS;
		}
		else
		{
			UserStage = Stage.ZERO;
		}
	}

	private void OnRoundEnds(Stage winnerStage)
	{
		if (winnerStage == UserStage)
		{
			Score++;
		}
	}





}
