using UnityEngine;
using System.Collections;

public class PlayerShooting_Bullet : MonoBehaviour 
{
	public float timeBetweenBullets = 0.15f;
	public float bulletLifeTime = 3f;
	public float shootForce = 10f;
	public Rigidbody bulletPrefab;
	
	float timer;
	AudioSource gunAudio;
	Light gunLight;
	ParticleSystem gunFlareParticles;
	float effectsDisplayTime = 0.03f;
	
	void Awake ()
	{
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
		gunFlareParticles = GetComponent<ParticleSystem> ();
	}

	void Update ()
	{
		timer += Time.deltaTime;
		
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
		{
			Shoot ();
			Invoke ("DisableEffects", effectsDisplayTime);
		}
	}

	void Shoot ()
	{
		timer = 0f;
		gunAudio.Play ();
		gunLight.enabled = true;
		gunFlareParticles.Stop ();
		gunFlareParticles.Play ();

		Rigidbody bullet = Instantiate (bulletPrefab, transform.position, transform.rotation * bulletPrefab.transform.rotation) as Rigidbody;
		bullet.AddForce(transform.forward * shootForce, ForceMode.Impulse);
		Destroy (bullet.gameObject, bulletLifeTime);
	}

	void DisableEffects ()
	{
		gunLight.enabled = false;
	}
}
