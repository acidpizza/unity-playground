using UnityEngine;
using System.Collections;

public class BulletSpec_FlamingAR : BulletSpec
{
	ParticleSystem bulletParticleSystem;
	ParticleCollisionEvent[] collisionEvents;

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
		bulletParticleSystem = GetComponent<ParticleSystem> ();
		collisionEvents = new ParticleCollisionEvent[16];

		Destroy (gameObject, 3f);
	}

	protected override void OnCollisionEnter(Collision other) 
	{
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = bulletParticleSystem.GetCollisionEvents (other, collisionEvents);
		if(numCollisionEvents > 0)
		{
			ParticleSystem hitParticles = Instantiate (hitParticlesPrefab, collisionEvents[0].intersection, hitParticlesPrefab.transform.rotation) as ParticleSystem;
			hitParticles.Play ();
			Destroy (hitParticles.gameObject, 0.3f);
		}

		EnemyHealth enemyHealth = other.GetComponent <EnemyHealth> ();
		if(enemyHealth != null)
		{
			enemyHealth.TakeDamage (damagePerShot);
			BulletTracker.AmmoHit(this);
		}
	}
}
