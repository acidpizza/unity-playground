using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour
{
	public enum Setting
	{
		Campaign, Survival
	};

	public static Setting setting = Setting.Campaign;

	void Awake()
	{
		BulletTracker.Clear ();
	}
}
