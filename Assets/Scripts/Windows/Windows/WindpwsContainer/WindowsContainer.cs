using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsContainer : MonoBehaviour
{
	private void Awake()
	{
		WindowManager.Instance.UpdateWindowContainer(this);
	}
}
