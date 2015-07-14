using UnityEngine;
using System.Collections;

public class GameConfig
{
	public enum Difficulty
	{
		Easy, Normal, Hard
	};

	public static Difficulty difficulty = Difficulty.Normal;

}
