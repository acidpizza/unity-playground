using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	Text scoreText_;
    int score_;
    
	public Transform pauseText_;

	public Transform powerup_AssaultRifle_;
	public Transform powerup_PulseRifle_;
	public Transform powerup_GrenadeLauncher_;
	public Transform powerup_Health_;

	public EnemySpawn zomBunnySpawner_;
	public EnemySpawn zomBearSpawner_;
	public EnemySpawn attackBotSpawner_;

	int count_zombunny_ =0;
	int count_zombear_ = 0;

	bool pauseGame_ = false;

    void Awake ()
    {
		scoreText_ = GameObject.Find ("ScoreText").GetComponent<Text>();
		score_ = 0;
		AddScore (0, "", transform); // Initialise score
    }

	void Update()
	{
		if(Input.GetButtonDown ("Fire2"))
		{
			if(!pauseGame_)
			{
				Time.timeScale = 0f;
				pauseGame_ = true;
				pauseText_.gameObject.SetActive(true);
				AudioListener.pause = true;
			}
			else
			{
				Time.timeScale = 1f;
				pauseGame_ = false;
				pauseText_.gameObject.SetActive(false);
				AudioListener.pause = false;
			}
		}
	}


	public void AddScore(int value, string enemyName, Transform enemyPosition)
	{
		score_ += value;
		scoreText_.text = "Score: " + score_;

		// PowerUp drop algo
		if(enemyName == "ZomBunny")
		{
			count_zombunny_++;
			if(count_zombunny_ % 8  == 0)
			{
				DropPowerUp(powerup_AssaultRifle_, enemyPosition);
			}
			else if(count_zombunny_ % 4 == 0)
			{
				DropPowerUp(powerup_PulseRifle_, enemyPosition);
			}
		}
		else if(enemyName == "ZomBear")
		{
			count_zombear_++;
			if(count_zombear_ % 4 == 0)
			{
				DropPowerUp(powerup_Health_, enemyPosition);
			}
			else if(count_zombear_ % 2 == 0)
			{
				DropPowerUp(powerup_GrenadeLauncher_, enemyPosition);
			}
		}

		// Game state algo
		if(count_zombunny_ == 0)
		{
			zomBunnySpawner_.spawnTime_ = 3.5f;
			zomBearSpawner_.spawnTime_ = 999f;
			attackBotSpawner_.spawnTime_ = 15f;
		}
		else if(count_zombunny_ == 20)
		{
			zomBearSpawner_.spawnTime_ = 17f;
		}
		else if(count_zombunny_ % 20 == 0)
		{
			zomBunnySpawner_.spawnTime_ *= 0.97f;
			zomBearSpawner_.spawnTime_ *= 0.97f;
			attackBotSpawner_.spawnTime_ *= 0.97f;
		}
	}

	void DropPowerUp(Transform powerup, Transform dropTransform)
	{
		Vector3 dropPosition = dropTransform.position;
		dropPosition.y = powerup.position.y;
		Instantiate (powerup, dropPosition, powerup.rotation);
	}
}
