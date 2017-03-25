using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractUser
{
	protected AbstractUser(Stage stage, string name)
	{
		UserStage = stage;
		Name = name;
	}
	public Stage UserStage;
	public string Name;

	public virtual void Play()
	{
		//Debug.Log("User with stage: " + UserStage + " is current");
	}

}
