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
		_cachedObjects = new Dictionary<Type, ScriptableObject>();
		LoadScrptblObjects();
	}
	private const string _sOSourcePath = "Assets/Resources/ScriptableObjects";
	private const string _fileExtention = ".asset";


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
		//DirectoryInfo soObjectsDirectory = new DirectoryInfo(_sOSourcePath);
		//FileInfo[] files = soObjectsDirectory.GetFiles("*" + _fileExtention, SearchOption.AllDirectories);
		//foreach (FileInfo file in files)
		//{
		//	string soFullPath = file.Directory + @"\" + file;
		//	string destinationString = "Assets\\Resources\\";
		//	string pathForLoad = soFullPath.Substring(soFullPath.LastIndexOf(destinationString) + destinationString.Length).Replace(_fileExtention, "");
		//	ScriptableObject asset = (ScriptableObject)Resources.Load(pathForLoad, typeof(ScriptableObject));
		//	_cachedObjects.Add(asset.GetType(), asset);
		//}
	}

	private bool ISSOCached(Type type)
	{
		if (_cachedObjects.Keys.Contains(type))
		{
			return true;
		}
		else return false;
	}


}
