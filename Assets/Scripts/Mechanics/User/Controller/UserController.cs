using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserController : MonoBehaviour
{
	private static UserController _instance;
	public static UserController Instance { get { return _instance = _instance ?? FindObjectOfType<UserController>(); } }
	private UserController() { }

	public AbstractUser currentUser { get; private set; }
	public List<AbstractUser> users { get; private set; }

	private void Awake()
	{
		CellController.CellChanged += OnCellChanged;
	}

	public void OnCellChanged()
	{
		currentUser = users.SingleOrDefault(user => user != currentUser);
		currentUser.Play();
	}


	public void CreateUsers<T, L>()
		where T : AbstractUser
		where L : AbstractUser
	{
		var signature_1 = new object[2];
		signature_1[0] = Stage.CROSS;
		var signature_2 = new object[2];
		signature_2[0] = Stage.ZERO;

		if (typeof(T) == typeof(User) && typeof(L) == typeof(User))
		{
			signature_1[1] = "User_1";
			signature_2[1] = "User_2";
		}
		else if (typeof(T) == typeof(Bot) && typeof(L) == typeof(Bot))
		{
			signature_1[1] = "Bot_1";
			signature_2[1] = "Bot_2";
		}
		else
		{
			signature_1[1] = (typeof(T) == typeof(User)) ? "User" : "BOT";
			signature_2[1] = (typeof(L) == typeof(User)) ? "User" : "BOT";
		}

		users = new List<AbstractUser>
		{
			(T)Activator.CreateInstance(typeof(T), signature_1),
			(L)Activator.CreateInstance(typeof(L), signature_2)
		};
		currentUser = users.SingleOrDefault(user => user.UserStage == Stage.CROSS);

		if (users.First().GetType() == typeof(Bot))
			currentUser.Play();
	}

	public string GetUserNameByStage(Stage stage)
	{
		var neededUser = users.SingleOrDefault(user => user.UserStage == stage);
		return neededUser.Name;
	}
}
