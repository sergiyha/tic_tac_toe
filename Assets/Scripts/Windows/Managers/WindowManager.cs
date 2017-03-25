using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Window;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
	private static WindowManager _instance;
	public static WindowManager Instance { get { return _instance = _instance ?? FindObjectOfType<WindowManager>(); } }

	public Window[] windows;

	public void CloseCurrentWindow()
	{
		windows.SingleOrDefault(window => window.gameObject.activeInHierarchy).CloseWindow();
	}

	public void GetWindow<T>(Hashtable table = null) where T : Window
	{
		var currWindow = windows.SingleOrDefault(window => window.GetType() == typeof(T));
		currWindow.OpenWindow(table);
		windows.ToList().ForEach(window =>
		{
			if (window.GetType() != typeof(T)) window.CloseWindow();
		});
	}
}
