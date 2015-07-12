using UnityEngine;
using System.Collections;

public class BulletSpec : MonoBehaviour 
{
	public ParticleSystem hitParticlesPrefab;

	public float timeBetweenBurst;			// Time between each burst
	public int burstRounds;					// Number of rounds in a burst (0 means no burst)
	public float timeBetweenBulletInBurst;	// Time between each burst bullet

	public int damagePerShot;
	public float bulletLifeTime;
	public float shootForce;
	public float shootSpread;
	public int bulletCount;
	public string bulletName;
	public AudioClip shootAudio;

	public bool isProjectile = false;
	public float liftForce = 0f;
	public float forwardTorque = 0f;

	protected virtual void OnCollisionEnter(Collision other) 
	{
		ParticleSystem hitParticles = Instantiate (hitParticlesPrefab, other.contacts [0].point, hitParticlesPrefab.transform.rotation) as ParticleSystem;
		hitParticles.Play ();
		Destroy (hitParticles.gameObject, 0.3f);


		EnemyHealth enemyHealth = other.collider.GetComponent <EnemyHealth> ();
        if(enemyHealth != null)
        {
			// Special cases
			if(enemyHealth.enemyName == "ZomBear" && bulletName == "Pulse Rifle")
			{
				enemyHealth.TakeDamage (damagePerShot/3); // ZomBear has resistance to pulse rifle

			}
			else if(enemyHealth.enemyName == "AttackBot" && (bulletName == "Assault Rifle" || bulletName == "Pistol"))
			{
				enemyHealth.TakeDamage (damagePerShot/2); // AttackBot has resistance to mechanical bullets
				
			}
			else
			{
				enemyHealth.TakeDamage (damagePerShot);
			}

        }


		Destroy(gameObject);
	}
}
