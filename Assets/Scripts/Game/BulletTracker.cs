using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletTracker : MonoBehaviour
{
	public class BulletStats
	{
		public BulletStats(string newBulletName, int newAmmoCollected)
		{
			bulletName = newBulletName;
			ammoCollected = newAmmoCollected;
		}
		
		public string bulletName;
		public int ammoCollected;
		public int ammoShot;
		public int ammoHit;
	};

	public class EnemyKillStats
	{
		public EnemyKillStats(string newEnemyName)
		{
			enemyName = newEnemyName;
			killCount = 1;
		}

		public string enemyName;
		public int killCount;
	}

	public class EnemyDamageStats
	{
		public EnemyDamageStats(string newEnemyName, int damage)
		{
			enemyName = newEnemyName;
			damageCount = damage;
		}
		
		public string enemyName;
		public int damageCount;
	};

	public static void Clear()
	{
		bulletStatsList.Clear ();
		enemyKillStatsList.Clear ();
		enemyDamageStatsList.Clear ();
	}

#region BulletStats
	public static void CollectAmmo(BulletSpec bulletSpec)
	{
		if(bulletSpec != null)
		{
			foreach( BulletStats bulletStat in bulletStatsList)
			{
				if(bulletSpec.bulletName.Equals(bulletStat.bulletName))
				{
					bulletStat.ammoCollected += bulletSpec.bulletCount;
					return; // bulletStat found
				}
			}
			
			// New bullet type collected
			bulletStatsList.Add(new BulletStats(bulletSpec.bulletName, bulletSpec.bulletCount));
		}
		else
		{
			Debug.Log ("Cannot read bulletSpec in BulletTracker.CollectAmmo().");
		}
	}

	public static void AmmoShot(BulletSpec bulletSpec)
	{
		if(bulletSpec != null)
		{
			foreach( BulletStats bulletStat in bulletStatsList)
			{
				if(bulletSpec.bulletName.Equals(bulletStat.bulletName))
				{
					bulletStat.ammoShot++;
					return; // bulletStat found
				}
			}
			
			Debug.Log ("Cannot find bullet in BulletTracker.AmmoShot().");
		}
		else
		{
			Debug.Log ("Cannot read bulletSpec in BulletTracker.AmmoShot().");
		}
	}

	public static void AmmoHit(string bulletName)
	{
		foreach( BulletStats bulletStat in bulletStatsList)
		{
			if(bulletName.Equals(bulletStat.bulletName))
			{
				bulletStat.ammoHit++;
				return; // bulletStat found
			}
		}
		
		Debug.Log ("Cannot find bullet in BulletTracker.AmmoHit().");
	}

	public static void AmmoHit(BulletSpec bulletSpec)
	{
		if(bulletSpec != null)
		{
			AmmoHit(bulletSpec.bulletName);
		}
		else
		{
			Debug.Log ("Cannot read bulletSpec in BulletTracker.AmmoHit().");
		}
	}

#endregion

#region EnemyKillStats
	public static void EnemyKill(string enemyName)
	{
		foreach( EnemyKillStats enemyKillStats in enemyKillStatsList)
		{
			if(enemyKillStats.enemyName.Equals(enemyName))
			{
				enemyKillStats.killCount++;
				return; // enemyKillStats found
			}
		}
		
		// New enemy type killed
		enemyKillStatsList.Add(new EnemyKillStats(enemyName));
	}
#endregion

#region EnemyDamageStats
	public static void EnemyDamage(string enemyName, int damage)
	{
		foreach( EnemyDamageStats enemyDamageStats in enemyDamageStatsList)
		{
			if(enemyDamageStats.enemyName.Equals(enemyName))
			{
				enemyDamageStats.damageCount += damage;
				return; // enemyDamageStats found
			}
		}
		
		// New enemy type killed
		enemyDamageStatsList.Add(new EnemyDamageStats(enemyName, damage));
	}
#endregion

	public static List<BulletStats> bulletStatsList = new List<BulletStats> ();
	public static List<EnemyKillStats> enemyKillStatsList = new List<EnemyKillStats> ();

	public static List<EnemyDamageStats> enemyDamageStatsList = new List<EnemyDamageStats> ();
};
