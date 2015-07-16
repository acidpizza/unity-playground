using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	Text scoreText_;
    int score_;
    
	Text statsText_;
	bool statsUpdated_ = false;

	public Transform pauseText_;

	public Transform powerup_AssaultRifle_;
	public Transform powerup_PulseRifle_;
	public Transform powerup_GrenadeLauncher_;
	public Transform powerup_Health_;

	public EnemySpawn zomBunnySpawner_;
	public EnemySpawn zomBearSpawner_;
	public EnemySpawn attackBotSpawner_;
	
	int count_zombunny_kills_ = 0;
	int count_zombear_kills_ = 0;
	int count_attackBot_kills_ = 0;

	bool pauseGame_ = false;
	
	Transform player_;
	PlayerHealth playerHealth_;
	
	void Awake ()
    {
		scoreText_ = GameObject.Find ("ScoreText").GetComponent<Text>();
		score_ = 0;
		AddScore(0, "", transform); // Initialise score and game state

		statsText_ = GameObject.Find ("StatsText").GetComponent<Text> ();

		player_ = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth_ = player_.GetComponent <PlayerHealth> ();
	}
	
	void Update()
	{
		if(Input.GetButtonDown ("Pause"))
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

		if(playerHealth_.IsDead() && !statsUpdated_)
		{
			statsUpdated_ = true;
			Invoke("FormatStats", 5f);
		}
	}
	
	public void AddScore(int value, string enemyName, Transform enemyPosition)
	{
		if(!enemyName.Equals(""))
		{
			BulletTracker.EnemyKill (enemyName);
		}

		score_ += value;
		scoreText_.text = "Score: " + score_;

		// PowerUp drop algo
		if(enemyName == "ZomBunny")
		{
			count_zombunny_kills_++;
			if(count_zombunny_kills_ % 8  == 0)
			{
				DropPowerUp(powerup_AssaultRifle_, enemyPosition);
			}
			else if(count_zombunny_kills_ % 4 == 0)
			{
				DropPowerUp(powerup_PulseRifle_, enemyPosition);
			}
		}
		else if(enemyName == "ZomBear")
		{
			count_zombear_kills_++;
			if(count_zombear_kills_ % 4 == 0)
			{
				DropPowerUp(powerup_Health_, enemyPosition);
			}
			else if(count_zombear_kills_ % 2 == 0)
			{
				DropPowerUp(powerup_GrenadeLauncher_, enemyPosition);
			}
		}
		else if(enemyName == "AttackBot")
		{
			count_attackBot_kills_++;
		}

		// Game state algo
		if(count_zombunny_kills_ == 0)
		{
			zomBunnySpawner_.spawnTime_ = 3.5f;
			zomBearSpawner_.spawnTime_ = 999f;
			attackBotSpawner_.spawnTime_ = 15f;
		}
		else if(count_zombunny_kills_ == 20)
		{
			zomBearSpawner_.spawnTime_ = 17f;
		}
		else if(count_zombunny_kills_ % 20 == 0)
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

	public void FormatStats()
	{
		statsText_.text = "";

		statsText_.text += "|     Weapon     | Collected |  Shot  |  Hit  | % Used | % Accuracy |\n";
		foreach( BulletTracker.BulletStats bulletStat in BulletTracker.bulletStatsList)
		{
			string used = bulletStat.ammoCollected == 0 ? "NA" : (100.0f * bulletStat.ammoShot / bulletStat.ammoCollected).ToString("0.00");
			string accuracy = bulletStat.ammoShot == 0 ? "NA" : (100.0f * bulletStat.ammoHit / bulletStat.ammoShot).ToString("0.00");
			statsText_.text += string.Format("|{0,-16}|{1,-11}|{2,-8}|{3,-7}|{4,-8:N2}|{5,-12:N2}|\n", 
			                                 bulletStat.bulletName,bulletStat.ammoCollected,bulletStat.ammoShot,bulletStat.ammoHit, 
			                                 used, accuracy);
		}

		statsText_.text += "\n";

		statsText_.text += "|   Enemy   | Kills |\n";
		foreach( BulletTracker.EnemyKillStats enemyKillStats in BulletTracker.enemyKillStatsList)
		{
			statsText_.text += string.Format ("|{0,-11}|{1,-7}|\n", enemyKillStats.enemyName, enemyKillStats.killCount);
		}

		statsText_.text += "\n";
		
		statsText_.text += "| Enemy Attack  | Damage |\n";
		foreach( BulletTracker.EnemyDamageStats enemyDamageStats in BulletTracker.enemyDamageStatsList)
		{
			statsText_.text += string.Format ("|{0,-15}|{1,-8}|\n", enemyDamageStats.enemyName, enemyDamageStats.damageCount);
		}
	}
}
