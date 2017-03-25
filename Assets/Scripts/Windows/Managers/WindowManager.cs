using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Window;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
	private static WindowManager _instance;
	public static WindowManager Instance { get { return _instance = _instance ?? FindObjectOfType<WindowManager>(); } }
	public WindowManager() { }
	public GameObject WindowsPlace;


	public Window[] windows;

	private Dictionary<Type, Window> _createdWindows;

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


		_createdWindows = new Dictionary<Type, Window>();
	}

	public void CloseCurrentWindow()
	{
		windows.SingleOrDefault(window => window.gameObject.activeInHierarchy).CloseWindow();
	}

	public void GetWindow<T>(IInputWindowPatamerer parameter = null) where T : Window
	{
		if (_createdWindows.Keys.Contains(typeof(T)))
		{
			_createdWindows[typeof(T)].OpenWindow(parameter);
		}
		else
		{
			var currWindow = windows.SingleOrDefault(window => window.GetType() == typeof(T));
			var neededWindow = Instantiate(currWindow) as Window;
			neededWindow.transform.SetParent(WindowsPlace.transform, false);
			_createdWindows.Add(typeof(T), neededWindow);
			neededWindow.OpenWindow(parameter);
		}
	}
}
