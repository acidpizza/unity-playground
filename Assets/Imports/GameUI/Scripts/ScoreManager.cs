using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    static int score;
    static Text text;

    void Awake ()
    {
        text = GetComponent <Text> ();
		score = 0;
		AddScore (0);
    }

	public static void AddScore(int value)
	{
		score += value;
		text.text = "Score: " + score;
	}
}
