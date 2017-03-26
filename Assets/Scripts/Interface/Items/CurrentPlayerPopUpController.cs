using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayerPopUpController : MonoBehaviour
{

	public CurrentPlayerPopUpView View;

	public void InitPopUpCurrentUserBar(string userName)
	{
		View.InitItem(userName);
		Show();
		StartCoroutine(HidePopUp());
	}

	private IEnumerator HidePopUp()
	{
		yield return new WaitForSeconds(4);
		this.gameObject.SetActive(false);
	}

	private void Show()
	{
		this.gameObject.SetActive(true);
	}

}
