using UnityEngine;
using System.Collections;

public class EnemyShooting_AttackBot : EnemyShooting 
{	
	public Rigidbody bulletPrefabLeft;
	public Rigidbody bulletPrefabRight;
	public Light gunLightLeft;
	public Light gunLightRight;
	public ParticleSystem gunFlareParticlesLeft;
	public ParticleSystem gunFlareParticlesRight;

	float timer = 0f;
	AudioSource gunAudio;
	float effectsDisplayTime = 0.03f;
	EnemyBulletSpec bulletSpec; // only need one instance of the 2 bullets

	// Use this for initialization
	void Awake () 
	{
		gunAudio = GetComponent<AudioSource> ();
		bulletSpec = bulletPrefabLeft.GetComponent<EnemyBulletSpec> ();
		timer = bulletSpec.timeBetweenBurst;
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

		Vector3 localOffset = new Vector3(bulletPrefabLeft.transform.position.x,0,0);
		Vector3 worldOffset = transform.rotation * localOffset;
		Vector3 bulletPosition = transform.position + worldOffset;
		Rigidbody bulletLeft = Instantiate (bulletPrefabLeft, bulletPosition, transform.rotation * bulletPrefabLeft.transform.rotation) as Rigidbody;
		bulletLeft.AddForce(transform.forward * bulletSpec.shootForce, ForceMode.Impulse);
		Destroy (bulletLeft.gameObject, bulletSpec.bulletLifeTime);

		localOffset = new Vector3(bulletPrefabRight.transform.position.x,0,0);
		worldOffset = transform.rotation * localOffset;
		bulletPosition = transform.position + worldOffset;
		Rigidbody bulletRight = Instantiate (bulletPrefabRight, bulletPosition, transform.rotation * bulletPrefabRight.transform.rotation) as Rigidbody;
		bulletRight.AddForce(transform.forward * bulletSpec.shootForce, ForceMode.Impulse);
		Destroy (bulletRight.gameObject, bulletSpec.bulletLifeTime);

		Invoke ("DisableEffects", effectsDisplayTime);	
	}

	void EnableEffects()
	{
		gunAudio.Play ();
		gunLightLeft.enabled = true;
		gunLightRight.enabled = true;
		gunFlareParticlesLeft.Stop ();
		gunFlareParticlesLeft.Play ();
		gunFlareParticlesRight.Stop ();
		gunFlareParticlesRight.Play ();
	}

	void DisableEffects ()
	{
		gunLightLeft.enabled = false;
		gunLightRight.enabled = false;
	}
}
