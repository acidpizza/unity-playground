using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrenadeExplode : MonoBehaviour
{
	public int explosionDamage;
	public float explosionRadius;

	ParticleSystem explosionParticles;
	AudioSource explosionAudio;


	void Awake()
	{
		explosionParticles = GetComponent<ParticleSystem> ();
		explosionAudio = GetComponent<AudioSource> ();
		Explode ();
	}

	public void Explode()
	{
		explosionAudio.Play();
		explosionParticles.Play ();

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
		int i = 0;
		while (i < hitColliders.Length) 
		{
			EnemyHealth enemyHealth = hitColliders[i].GetComponent <EnemyHealth> ();
			if(enemyHealth != null && !hitColliders[i].isTrigger)
			{
				enemyHealth.TakeDamage(explosionDamage);
			}
			i++;
		}

		Destroy (gameObject, 5f);
	}
}
