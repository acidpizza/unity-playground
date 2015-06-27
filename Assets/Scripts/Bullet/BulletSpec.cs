using UnityEngine;
using System.Collections;

public class BulletSpec : MonoBehaviour 
{
	public ParticleSystem hitParticlesPrefab;

	public float timeBetweenBurst;				// Time between each burst
	public int burstRounds;						// Number of rounds in a burst (0 means no burst)
	public float timeBetweenBulletInBurst;		// Time between each burst bullet

	public int damagePerShot;
	public float bulletLifeTime;
	public float shootForce;
	public float shootSpread;

	void OnCollisionEnter(Collision other) 
	{
		ParticleSystem hitParticles = Instantiate (hitParticlesPrefab, other.contacts [0].point, hitParticlesPrefab.transform.rotation) as ParticleSystem;
		hitParticles.Play ();
		Destroy (hitParticles.gameObject, 0.3f);


		EnemyHealth enemyHealth = other.collider.GetComponent <EnemyHealth> ();
        if(enemyHealth != null)
        {
            enemyHealth.TakeDamage (damagePerShot);
        }


		Destroy(gameObject);
	}

	public float GetTimeBetweenBurst()
	{
		return timeBetweenBurst;
	}

	public int GetBurstRounds()
	{
		return burstRounds;
	}

	public float GetTimeBetweenBulletInBurst()
	{
		return timeBetweenBulletInBurst;
	}

	public float GetBulletLifeTime()
	{
		return bulletLifeTime;
	}

	public float GetShootForce()
	{
		return shootForce;
	}

	public float GetShootSpread()
	{
		return shootSpread;
	}
}
