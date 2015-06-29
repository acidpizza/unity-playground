using UnityEngine;
using System.Collections;

public class BulletSpec : MonoBehaviour 
{
	public ParticleSystem hitParticlesPrefab;

	public float timeBetweenBurst{ get; protected set;}	// Time between each burst
	public int burstRounds{ get; protected set;}					// Number of rounds in a burst (0 means no burst)
	public float timeBetweenBulletInBurst{ get; protected set;}	// Time between each burst bullet

	public int damagePerShot{ get; protected set;}
	public float bulletLifeTime{ get; protected set;}
	public float shootForce{ get; protected set;}
	public float shootSpread{ get; protected set;}
	public int bulletCount{ get; protected set;}
	public string bulletName{ get; protected set;}

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
}
