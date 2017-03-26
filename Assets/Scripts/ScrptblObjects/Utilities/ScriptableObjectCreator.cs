#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SU = ScriptableObjectUtility;

public class ScriptableObjectCreator : MonoBehaviour
{

	[MenuItem("Create/GameVariations")]
	static void CreateGameVariationsSO()
	{
		SU.CreateAsset<GameVariationsScrptblObject>();
	}

	[MenuItem("Create/Probability")]
	static void CreateProbabilitySO()
	{
		SU.CreateAsset<ProbabilityScriptableObject>();
	}
}
#endif
