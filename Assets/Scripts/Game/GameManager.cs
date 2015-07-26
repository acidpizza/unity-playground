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
	public EnemySpawn meteorSpawner_;

	public Transform bossSpawnPoint_;
	public Transform meteorHellephant_;
	public Transform meteorHellephantNormal_;

	int count_zombunny_kills_ = 0;
	int count_zombear_kills_ = 0;
	int count_attackBot_kills_ = 0;

	int count_zombunny_drop_AR = 0;
	int count_zombunny_drop_pulse = 0;
	int count_zombear_drop_grenade = 0;
	int count_zombear_drop_health = 0;

	bool pauseGame_ = false;
	AudioSource audioSource;

	Transform player_;
	PlayerHealth playerHealth_;

	bool win_ = false;

	void Awake ()
    {
		audioSource = GetComponent<AudioSource> ();

		scoreText_ = GameObject.Find ("ScoreText").GetComponent<Text>();
		score_ = 0;
		AddScore(0, "", transform); // Initialise score and game state

		statsText_ = GameObject.Find ("StatsText").GetComponent<Text> ();

		player_ = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth_ = player_.GetComponent <PlayerHealth> ();

		SetPowerupDropConditions ();
//count_zombunny_kills_ = 17;
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

		if((playerHealth_.IsDead() || win_) && !statsUpdated_)
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
			if(count_zombunny_kills_ % count_zombunny_drop_AR  == 0)
			{
				DropPowerUp(powerup_AssaultRifle_, enemyPosition);
			}
			else if(count_zombunny_kills_ % count_zombunny_drop_pulse == 0)
			{
				DropPowerUp(powerup_PulseRifle_, enemyPosition);
			}
		}
		else if(enemyName == "ZomBear")
		{
			count_zombear_kills_++;

			if(count_zombear_kills_ % count_zombear_drop_health == 0)
			{
				DropPowerUp(powerup_Health_, enemyPosition);
			}
			else if(count_zombear_kills_ % count_zombear_drop_grenade == 0)
			{
				DropPowerUp(powerup_GrenadeLauncher_, enemyPosition);
			}
		}
		else if(enemyName == "AttackBot")
		{
			count_attackBot_kills_++;
		}

		// Game state algo
		if(enemyName == "ZomBunny")
		{
			if(GameConfig.setting == GameConfig.Setting.Campaign)
			{
				UpdateGameState_Campaign();
			}
			else if(GameConfig.setting == GameConfig.Setting.Survival)
			{
				UpdateGameState_Survival();
			}
		}
	}

	void UpdateGameState_Campaign()
	{
		if(count_zombunny_kills_ == 0)
		{
			zomBunnySpawner_.spawnTime_ = 2f;
			zomBearSpawner_.spawnTime_ = 999f;
			attackBotSpawner_.spawnTime_ = 999f;
		}
		else if(count_zombunny_kills_ == 3) // AttackBot Appearance
		{
			attackBotSpawner_.spawnTime_ = 12f;
			attackBotSpawner_.gameObject.SetActive(true);
		}
		else if(count_zombunny_kills_ == 12) // Mage Appearance
		{
			zomBearSpawner_.spawnTime_ = 15f;
			zomBearSpawner_.gameObject.SetActive(true);
		}
		else if(count_zombunny_kills_ == 18) // Boss Appearance
		{	
			attackBotSpawner_.gameObject.SetActive(true);
			zomBearSpawner_.gameObject.SetActive(true);
			zomBunnySpawner_.spawnTime_ = 5f;
			attackBotSpawner_.spawnTime_ = 15f;
			zomBearSpawner_.spawnTime_ = 17f;
			Instantiate (meteorHellephant_, bossSpawnPoint_.position, meteorHellephant_.rotation);
		}
	}

	public void ActivateBossSecondForm()
	{
		zomBunnySpawner_.enabled = false;
		
		zomBearSpawner_.burstSpawn_ = 2;
		zomBearSpawner_.spawnTime_ = 33f;
		zomBearSpawner_.SpawnNow ();
		
		attackBotSpawner_.burstSpawn_ = 2;
		attackBotSpawner_.spawnTime_ = 29f;
		attackBotSpawner_.SpawnNow ();
		
		meteorSpawner_.burstSpawn_ = 5;
		meteorSpawner_.spawnTime_ = 25f;
		meteorSpawner_.SpawnNow ();
		meteorSpawner_.gameObject.SetActive(true);
		
		audioSource.Play ();
	}

	void UpdateGameState_Survival()
	{
		if(count_zombunny_kills_ == 0)
		{
			zomBunnySpawner_.spawnTime_ = 2f;
			zomBearSpawner_.spawnTime_ = 999f;
			attackBotSpawner_.spawnTime_ = 999f;
		}
		else if(count_zombunny_kills_ == 3)
		{
			attackBotSpawner_.spawnTime_ = 13f;
			attackBotSpawner_.gameObject.SetActive(true);
		}
		else if(count_zombunny_kills_ == 12)
		{
			zomBearSpawner_.spawnTime_ = 17f;
			zomBearSpawner_.gameObject.SetActive(true);
		}
		else if(count_zombunny_kills_ == 18)
		{
			zomBunnySpawner_.spawnTime_ = 3.5f;
		}
		else if(count_zombunny_kills_ % 20 == 0)
		{
			zomBunnySpawner_.spawnTime_ *= 0.97f;
			zomBearSpawner_.spawnTime_ *= 0.97f;
			attackBotSpawner_.spawnTime_ *= 0.97f;
			Instantiate (meteorHellephantNormal_, bossSpawnPoint_.position, meteorHellephantNormal_.rotation);
		}
	}

	void SetPowerupDropConditions()
	{
		if(GameConfig.setting == GameConfig.Setting.Campaign)
		{
			count_zombunny_drop_AR = 6;
			count_zombunny_drop_pulse = 3;
			count_zombear_drop_health = 2;
			count_zombear_drop_grenade = 1;
		}
		else if(GameConfig.setting == GameConfig.Setting.Survival)
		{
			count_zombunny_drop_AR = 6;
			count_zombunny_drop_pulse = 3;
			count_zombear_drop_health = 4;
			count_zombear_drop_grenade = 2;
		}
	}
	
	public void FormatStats()
	{
		statsText_.text = "";
		if(win_)
		{
			statsText_.text += "Congrats for clearing the game! Hope you had fun playing =P\n\n";
		}

		if(GameConfig.setting == GameConfig.Setting.Campaign)
		{
			statsText_.text += "Campaign Mode Stats: \n\n";
		}
		else if(GameConfig.setting == GameConfig.Setting.Survival)
		{
			statsText_.text += "Survival Mode Stats: \n\n";
		}

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
		
		statsText_.text += "|  Enemy Attack  | Damage |\n";
		foreach( BulletTracker.EnemyDamageStats enemyDamageStats in BulletTracker.enemyDamageStatsList)
		{
			statsText_.text += string.Format ("|{0,-16}|{1,-8}|\n", enemyDamageStats.enemyName, enemyDamageStats.damageCount);
		}
	}

	public void WinGame()
	{
		meteorSpawner_.enabled = false;
		zomBunnySpawner_.enabled = false;
		attackBotSpawner_.enabled = false;
		zomBearSpawner_.enabled = false;
		win_ = true;
		playerHealth_.gameObject.GetComponent<PlayerMovement>().enabled = false;
		playerHealth_.gameObject.GetComponentInChildren<PlayerShooting>().enabled = false;
		playerHealth_.Win ();
	}

	void DropPowerUp(Transform powerup, Transform dropTransform)
	{
		Vector3 dropPosition = dropTransform.position;
		dropPosition.y = powerup.position.y;
		Instantiate (powerup, dropPosition, powerup.rotation);
	}
}
