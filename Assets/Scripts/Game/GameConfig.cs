using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour
{
	public enum Difficulty
	{
		Easy, Normal, Hard
	};

	public static Difficulty difficulty = Difficulty.Normal;

	void Awake()
	{
		BulletTracker.Clear ();
	}
}
