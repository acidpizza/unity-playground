using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour 
{
	public Rigidbody bulletPrefab;
	float timeBetweenBullets;
	//int burstRounds;
	//float burstTime;
	float bulletLifeTime;
	float shootForce;
	float shootSpread;
	int bulletSpreadDirection = 1; // this will toggle between 1 and -1 for rotation direction

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
		ChangeWeapon (bulletPrefab);
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

	public void ChangeWeapon(Rigidbody newBulletPrefab)
	{
		bulletPrefab = newBulletPrefab;
		BulletSpec bulletSpec = bulletPrefab.GetComponent <BulletSpec> ();
		if (bulletSpec != null) 
		{
			timeBetweenBullets = bulletSpec.GetTimeBetweenBullets();
			//burstRounds = bulletSpec.GetBurstRounds();
			//burstTime = bulletSpec.GetBurstTime();
			bulletLifeTime = bulletSpec.GetBulletLifeTime();
			shootForce = bulletSpec.GetShootForce();
			shootSpread = bulletSpec.GetShootSpread();
		}
		else
		{
			Debug.Log ("Cannot change weapon");
		}
	}

	void Shoot ()
	{
		timer = 0f;
		gunAudio.Play ();
		gunLight.enabled = true;
		gunFlareParticles.Stop ();
		gunFlareParticles.Play ();

		Transform bulletTransform = transform;
		if(shootSpread != 0)
		{
			bulletTransform.Rotate(new Vector3(0,1,0) * Random.Range (0, shootSpread) * bulletSpreadDirection);
			bulletSpreadDirection *= -1; // Always ensure each successive bullet is rotated in the opposite direction
		}

		Rigidbody bullet = Instantiate (bulletPrefab, bulletTransform.position, bulletTransform.rotation * bulletPrefab.transform.rotation) as Rigidbody;
		bullet.AddForce(bulletTransform.forward * shootForce, ForceMode.Impulse);
		Destroy (bullet.gameObject, bulletLifeTime);
	}

	void DisableEffects ()
	{
		gunLight.enabled = false;
	}
}
