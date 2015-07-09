using UnityEngine;
using System.Collections;

public class EnemyShooting_ZomBear : EnemyShooting 
{	
	public Rigidbody magicPrefab;
	public Light magicLight;
	public ParticleSystem magicParticles;

	float timer = 0f;
	AudioSource magicAudio;
	float effectsDisplayTime = 0.03f;
	EnemyBulletSpec bulletSpec; // only need one instance of the 2 bullets

	// Use this for initialization
	void Awake () 
	{
		magicAudio = GetComponent<AudioSource> ();
		bulletSpec = magicPrefab.GetComponent<EnemyBulletSpec> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(IsShooting && timer >= bulletSpec.timeBetweenBurst)
		{
			Shoot ();
		}
	}

	void Shoot()
	{
		timer = 0f;
		EnableEffects ();

		Vector3 localOffset = new Vector3(magicPrefab.transform.position.x,0,0);
		Vector3 worldOffset = transform.rotation * localOffset;
		Vector3 bulletPosition = transform.position + worldOffset;
		Rigidbody bulletLeft = Instantiate (magicPrefab, bulletPosition, transform.rotation * magicPrefab.transform.rotation) as Rigidbody;
		bulletLeft.AddForce(transform.forward * bulletSpec.shootForce, ForceMode.Impulse);
		Destroy (bulletLeft.gameObject, bulletSpec.bulletLifeTime);

		Invoke ("DisableEffects", effectsDisplayTime);	
	}

	void EnableEffects()
	{
		magicAudio.Play ();
		magicLight.enabled = true;
		magicParticles.Stop ();
		magicParticles.Play ();
	}

	void DisableEffects ()
	{
		magicLight.enabled = false;
	}
}
