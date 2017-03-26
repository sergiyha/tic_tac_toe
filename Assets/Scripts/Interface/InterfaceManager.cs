using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
	private static InterfaceManager _instance;
	public static InterfaceManager Instance { get { return _instance = _instance ?? FindObjectOfType<InterfaceManager>(); } }
	public GameObject _interfaceContainer;
	public InterfaceScoreBarController scoreBar;
	public CurrentPlayerPopUpController popUP;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}


		GameManager.Instance.RoundBegins += OnRoundBegins;
		GameManager.Instance.RoundEnds += OnRoundEnds;


	}

	private void OnDestroy()
	{
		GameManager.Instance.RoundBegins -= OnRoundBegins;
		GameManager.Instance.RoundEnds -= OnRoundEnds;
	}

	public void UpdateInterface()
	{
		scoreBar.InitScoreItems(UserController.Instance.users);
		ShowInterface();
	}


	private void HideInterface()
	{
		_interfaceContainer.gameObject.SetActive(false);
	}

	private void OnRoundBegins(Stage stage)
	{
		UpdateInterface();
		popUP.InitPopUpCurrentUserBar(UserController.Instance.GetUserByStage(Stage.CROSS).GetName());
	}

	void OnRoundEnds(Stage stage)
	{
		HideInterface();
	}

	private void ShowInterface()
	{
		_interfaceContainer.gameObject.SetActive(true);
	}
}
