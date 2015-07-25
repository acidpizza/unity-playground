using UnityEngine;
using System.Collections;

public class BulletSpec_FlamingAR : BulletSpec
{
	public BulletSpec_FlamingAR()
	{
		timeBetweenBurst = 1.2f;
		burstRounds = 3;
		timeBetweenBulletInBurst = 0.1f;

		damagePerShot = 60;
		bulletLifeTime = 1.5f;
		shootForce = 10f;
		shootSpread = 1f;
		
		bulletCount = 30;
		bulletName = "Assault Rifle";
	}

	void Awake()
	{
		Destroy (gameObject, 3f);
	}

	protected override void OnCollisionEnter(Collision other) 
	{
	}

	void OnParticleCollision(GameObject other)
	{
		EnemyHealth enemyHealth = other.GetComponent <EnemyHealth> ();
		if(enemyHealth != null)
		{
			enemyHealth.TakeDamage (damagePerShot);
			BulletTracker.AmmoHit(this);
		}
	}
}
