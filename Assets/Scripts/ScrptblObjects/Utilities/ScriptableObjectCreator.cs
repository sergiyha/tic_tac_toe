#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SU = ScriptableObjectUtility;

public class ScriptableObjectCreator : MonoBehaviour
{

	[MenuItem("Create/GameVariations")]
	static void DoSomething()
	{
		SU.CreateAsset<GameVariationsScrptblObject>();
	}

}
#endif
