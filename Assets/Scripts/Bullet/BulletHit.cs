using UnityEngine;
using System.Collections;

public class BulletHit : MonoBehaviour 
{
	public ParticleSystem hitParticlesPrefab;

	public int damagePerShot = 20;

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
