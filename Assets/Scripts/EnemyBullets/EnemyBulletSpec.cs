using UnityEngine;
using System.Collections;

public class EnemyBulletSpec : MonoBehaviour 
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

	void OnCollisionEnter(Collision other) 
	{
		ParticleSystem hitParticles = Instantiate (hitParticlesPrefab, other.contacts [0].point, hitParticlesPrefab.transform.rotation) as ParticleSystem;
		hitParticles.Play ();
		Destroy (hitParticles.gameObject, 0.3f);


		PlayerHealth playerHealth = other.collider.GetComponent <PlayerHealth> ();
        if(playerHealth != null)
        {
            playerHealth.TakeDamage (damagePerShot);
        }


		Destroy(gameObject);
	}
}
