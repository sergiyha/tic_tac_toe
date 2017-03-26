using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
	private static ScriptableObjectManager _instance;
	public static ScriptableObjectManager Instance { get { return _instance = _instance ?? FindObjectOfType<ScriptableObjectManager>(); } }
	private ScriptableObjectManager() { }

	private Dictionary<Type, ScriptableObject> _cachedObjects;
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

		DontDestroyOnLoad(this);

		_cachedObjects = new Dictionary<Type, ScriptableObject>();
		LoadScrptblObjects();
	}

	public T GetObject<T>() where T : ScriptableObject
	{
		return _cachedObjects[typeof(T)] as T;
	}

	private void LoadScrptblObjects()
	{

		ScriptableObject[] assets = Resources.LoadAll<ScriptableObject>("ScriptableObjects");
		foreach (var so in assets)
		{
			_cachedObjects.Add(so.GetType(), so);
		}
	}
}
