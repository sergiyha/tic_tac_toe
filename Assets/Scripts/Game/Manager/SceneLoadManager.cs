using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
	private static SceneLoadManager _instance;
	public static SceneLoadManager Instance { get { return _instance = _instance ?? FindObjectOfType<SceneLoadManager>(); } }


	private const string ScenesPath = "Scenes/";

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
	}

	public void StartLoadScene(ScenesToLoad sceneToLoad, Action afterSceneLoadingCallback)
	{
		StartCoroutine(LoadGame(sceneToLoad, afterSceneLoadingCallback));

	}

	private IEnumerator LoadGame(ScenesToLoad sceneToload, Action afterSceneLoadingCallback)
	{
		Debug.Log("Loading Level");
		yield return SceneManager.LoadSceneAsync(ScenesPath + ((int)sceneToload).ToString());
		Debug.Log("Scene " + sceneToload.ToString() + " Loaded");
		afterSceneLoadingCallback();
	}

}

public enum ScenesToLoad
{
	PregameScene = 1, FirstScene = 2
}
