using UnityEngine;
using System.Collections;

public class BulletHit : MonoBehaviour 
{
	public ParticleSystem hitParticlesPrefab;

	void OnCollisionEnter(Collision other) 
	{
		ParticleSystem hitParticles = Instantiate (hitParticlesPrefab, other.contacts [0].point, hitParticlesPrefab.transform.rotation) as ParticleSystem;
		hitParticles.Play ();
		Destroy (hitParticles.gameObject, 0.3f);

		/*
		EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
        if(enemyHealth != null)
        {
            enemyHealth.TakeDamage (damagePerShot, shootHit.point);
        }
		*/

		Destroy(gameObject);
	}
}
