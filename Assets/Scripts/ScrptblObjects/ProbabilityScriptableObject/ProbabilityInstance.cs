using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProbabilityInstance
{
	public GameDifficulty Difficulty;
	public bool[] Probability = new bool[10];
}
