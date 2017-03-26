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

	private WindowsContainer _windowContainer;


	public Window[] windows;

	private Dictionary<Type, Window> _createdWindows;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			DestroyImmediate(this.gameObject);
		}
		else
		{
			_instance = this;
		}

		DontDestroyOnLoad(this);

		_createdWindows = new Dictionary<Type, Window>();

	}

	public void CloseCurrentWindow()
	{
		windows.SingleOrDefault(window => window.gameObject.activeInHierarchy).CloseWindow();
	}

	public void UpdateWindowContainer(WindowsContainer windowContainer)
	{
		if (_windowContainer == null)
		{
			Debug.Log(this.GetInstanceID());
			_windowContainer = windowContainer;
		}
	}

	public void GetWindow<T>(IInputWindowPatamerer parameter = null) where T : Window
	{
		if (_createdWindows.Keys.Contains(typeof(T)))
		{
			if (_createdWindows[typeof(T)] == null)
			{
				_createdWindows.Remove(typeof(T));
				InStantiateNewWindow<T>(parameter);
				return;
			}
			_createdWindows[typeof(T)].OpenWindow(parameter);
		}
		else
		{
			InStantiateNewWindow<T>(parameter);
		}
	}

	private void InStantiateNewWindow<T>(IInputWindowPatamerer parameter = null) where T : Window
	{
		var currWindow = windows.SingleOrDefault(window => window.GetType() == typeof(T));
		var neededWindow = Instantiate(currWindow) as Window;
		neededWindow.transform.SetParent(_windowContainer.transform, false);
		_createdWindows.Add(typeof(T), neededWindow);
		neededWindow.OpenWindow(parameter);
	}

	private void OnDisable()
	{
		_windowContainer = null;
	}
}
