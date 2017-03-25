using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : AbstractUser
{
	public User(Stage stage, string name) : base(stage, name)
	{
	}

	public override void Play()
	{
		base.Play();
	}
}
