using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	Text scoreText;
    int score;
    
	public Transform powerup_AssaultRifle;
	public Transform powerup_PulseRifle;
	public Transform powerup_GrenadeLauncher;
	public Transform powerup_Health;

	int count_zombunny;
	int count_zombear;


    void Awake ()
    {
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text>();
		score = 0;
		AddScore (0, "", transform);

		count_zombunny = 0;
		count_zombear = 0;
    }

	public void AddScore(int value, string enemyName, Transform enemyPosition)
	{
		score += value;
		scoreText.text = "Score: " + score;

		if(enemyName == "ZomBunny")
		{
			count_zombunny++;
			if(count_zombunny == 4)
			{
				DropPowerUp(powerup_AssaultRifle, enemyPosition);
			}
			else if(count_zombunny == 8)
			{
				count_zombunny = 0;
				DropPowerUp(powerup_PulseRifle, enemyPosition);
			}
		}
		else if(enemyName == "ZomBear")
		{
			count_zombear++;
			if(count_zombear == 2)
			{
				DropPowerUp(powerup_GrenadeLauncher, enemyPosition);
			}
			else if(count_zombear == 4)
			{
				count_zombear = 0;
				DropPowerUp(powerup_Health, enemyPosition);
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
