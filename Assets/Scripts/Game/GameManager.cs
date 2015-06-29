using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	Text scoreText;
    int score;
    
	public Transform powerup_AssaultRifle;
	public Transform powerup_PulseRifle;
	int count_zombunny;


    void Awake ()
    {
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text>();
		score = 0;
		AddScore (0, "", transform);

		count_zombunny = 0;
    }

	public void AddScore(int value, string enemyName, Transform enemyPosition)
	{
		score += value;
		scoreText.text = "Score: " + score;

		if(enemyName == "ZomBunny")
		{
			count_zombunny++;
			if(count_zombunny == 1)
			{
				DropPowerUp(powerup_AssaultRifle, enemyPosition);
			}
			else if(count_zombunny == 2)
			{
				count_zombunny = 0;
				DropPowerUp(powerup_PulseRifle, enemyPosition);
			}
		}
	}

	void DropPowerUp(Transform powerup, Transform dropTransform)
	{
		Vector3 dropPosition = dropTransform.position;
		dropPosition.y = powerup.position.y;
		Instantiate (powerup, dropPosition, powerup.rotation);
	}
}
