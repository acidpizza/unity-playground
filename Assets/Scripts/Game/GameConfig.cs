using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour
{
	public enum GameMode
	{
		Campaign, Survival
	};

	public enum Difficulty
	{
		Chill, Challenge, Hardcore
	};

	public static GameMode gameMode = GameMode.Campaign;
	public static Difficulty difficulty = Difficulty.Challenge;
	public static float zoom = 30f;

	void Awake()
	{
		BulletTracker.Clear ();
	}
}
