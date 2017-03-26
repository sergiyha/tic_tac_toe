using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceScoreBarController : MonoBehaviour
{

	public InterfaceScoreBarView View;
	public InterfaceScoreItem ScoreItemInstance;


	public void Awake()
	{
		View.Back += OnBack;
	}

	private void OnDestroy()
	{
		View.Back -= OnBack;
	}

	void OnBack()
	{
		SceneLoadManager.Instance.StartLoadScene(ScenesToLoad.PregameScene, () => { });
	}

	public void InitScoreItems(List<AbstractUser> users)
	{
		users.ForEach(user =>
		{
			var playerItem = Instantiate(ScoreItemInstance) as InterfaceScoreItem;
			playerItem.transform.SetParent(View.ItemsGrid.transform, false);
			playerItem.InitItem(user.GetName(), user.GetScore().ToString());
		});
	}

	public void OnDisable()
	{

		foreach (Transform item in View.ItemsGrid.transform)
		{
			Destroy(item.gameObject);
		}
	}
}

