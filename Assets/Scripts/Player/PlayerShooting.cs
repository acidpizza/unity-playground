using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour 
{
	public Rigidbody bulletPrefab;
	float timeBetweenBurst;
	int burstRounds;
	float timeBetweenBulletInBurst;
	float bulletLifeTime;
	float shootForce;
	float shootSpread;

	int bulletSpreadDirection = 1; // this will toggle between 1 and -1 for rotation direction
	int currentBurstCount = 0; // Track how many burst bullets have been shot so far

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

		if(Input.GetButton ("Fire1") && Time.timeScale != 0) // Want to fire and game is not paused
		{
			if(burstRounds != 0)
			{
				// Burst enabled
				if(timer >= timeBetweenBulletInBurst && currentBurstCount < burstRounds)
				{
					// In the midst of a burst
					currentBurstCount++;
					Shoot ();
					Invoke ("DisableEffects", effectsDisplayTime);
				}
				else if(currentBurstCount >= burstRounds)
				{
					// Hit burst limit -> need to wait for new burst
					if(timer >= timeBetweenBurst)
					{
						currentBurstCount = 1;
						Shoot ();
						Invoke ("DisableEffects", effectsDisplayTime);
					}
				}
			}
			else
			{
				// Burst disabled
				if(timer >= timeBetweenBurst)
				{
					Shoot ();
					Invoke ("DisableEffects", effectsDisplayTime);
				}
			}
		}
	}

	public void ChangeWeapon(Rigidbody newBulletPrefab)
	{
		bulletPrefab = newBulletPrefab;
		BulletSpec bulletSpec = bulletPrefab.GetComponent <BulletSpec> ();
		if (bulletSpec != null) 
		{
			timeBetweenBurst = bulletSpec.GetTimeBetweenBurst();
			burstRounds = bulletSpec.GetBurstRounds();
			timeBetweenBulletInBurst = bulletSpec.GetTimeBetweenBulletInBurst();
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
