using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour 
{
	struct AmmoTracker
	{
		public AmmoTracker(Rigidbody newBulletPrefab)
		{
			bulletPrefab = newBulletPrefab;
			bulletSpec = bulletPrefab.GetComponent <BulletSpec> ();
			if(bulletSpec != null)
			{
				bulletCount = bulletSpec.bulletCount;
				unlimitedAmmo = (bulletCount == 0);
			}
			else
			{
				bulletCount = 0;
				unlimitedAmmo = false;
				Debug.Log ("Cannot assign bulletSpec in AmmoTracker.");
			}
		}

		public bool AddAmmo(Rigidbody newBulletPrefab)
		{
			BulletSpec newBulletSpec = newBulletPrefab.GetComponent<BulletSpec> ();
			if(newBulletSpec != null)
			{
				if(bulletSpec.bulletName.Equals(newBulletSpec.bulletName))
				{
					bulletCount += newBulletSpec.bulletCount;
					// TODO: Update Ammo Count on UI
					return true;
				}
			}
			else
			{
				Debug.Log ("Cannot assign bulletSpec in AmmoTracker.AddAmmo().");
			}

			return false;
		}

		public bool AmmoAvailable()
		{
			return unlimitedAmmo || (bulletCount > 0);
		}

		public void ExpendAmmo()
		{
			if(!unlimitedAmmo && bulletCount > 0)
			{
				bulletCount --;
				// TODO: Update Ammo Count on UI
			}
		}

		public Rigidbody bulletPrefab;
		public BulletSpec bulletSpec;
		public int bulletCount;
		public bool unlimitedAmmo;
	};

	List<AmmoTracker> ammoStore = new List<AmmoTracker> ();
	int ammoStoreIndex = 0;
	AmmoTracker currentAmmoTracker;
	Rigidbody bulletPrefab;
	BulletSpec bulletSpec;

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
		CollectWeapon (bulletPrefab);
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && Time.timeScale != 0) // Want to fire and game is not paused
		{
			if(bulletSpec.burstRounds != 0)
			{
				// Burst enabled
				if(timer >= bulletSpec.timeBetweenBulletInBurst && currentBurstCount < bulletSpec.burstRounds)
				{
					// In the midst of a burst
					currentBurstCount++;
					Shoot ();
					Invoke ("DisableEffects", effectsDisplayTime);
				}
				else if(currentBurstCount >= bulletSpec.burstRounds)
				{
					// Hit burst limit -> need to wait for new burst
					if(timer >= bulletSpec.timeBetweenBurst)
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
				if(timer >= bulletSpec.timeBetweenBurst)
				{
					Shoot ();
					Invoke ("DisableEffects", effectsDisplayTime);
				}
			}
		}
	}

	public void CollectWeapon(Rigidbody newBulletPrefab)
	{
		foreach (AmmoTracker ammoTracker in ammoStore)
		{
			if(ammoTracker.AddAmmo(newBulletPrefab))
			{
				bulletPrefab = ammoTracker.bulletPrefab;
				bulletSpec = ammoTracker.bulletSpec;

				// TODO: Update UI
				return;
			}
		}

		{
			// Cannot find, add new AmmoTracker -> UI does not need to be updated.
			ammoStore.Add(new AmmoTracker(newBulletPrefab));
			AmmoTracker ammoTracker = ammoStore[ammoStore.Count - 1];
			bulletPrefab = ammoTracker.bulletPrefab;
			bulletSpec = ammoTracker.bulletSpec;
			if (bulletSpec == null) 
			{
				Debug.Log ("Cannot collect weapon");
			}
		}
	}

	public void ChangeWeapon()
	{
		ammoStoreIndex = (ammoStoreIndex + 1) % ammoStore.Count;
		currentAmmoTracker = ammoStore [ammoStoreIndex];
		bulletPrefab = currentAmmoTracker.bulletPrefab;
		bulletSpec = currentAmmoTracker.bulletSpec;
		if (bulletSpec != null) 
		{
			//TODO: Update UI
		}
		else
		{
			Debug.Log ("Cannot change weapon");
		}
	}

	void Shoot ()
	{
		if(currentAmmoTracker.AmmoAvailable())
		{
			timer = 0f;
			gunAudio.Play ();
			gunLight.enabled = true;
			gunFlareParticles.Stop ();
			gunFlareParticles.Play ();

			Transform bulletTransform = transform;
			if(bulletSpec.shootSpread != 0)
			{
				bulletTransform.Rotate(new Vector3(0,1,0) * Random.Range (0, bulletSpec.shootSpread) * bulletSpreadDirection);
				bulletSpreadDirection *= -1; // Always ensure each successive bullet is rotated in the opposite direction
			}

			Rigidbody bullet = Instantiate (bulletPrefab, bulletTransform.position, bulletTransform.rotation * bulletPrefab.transform.rotation) as Rigidbody;
			bullet.AddForce(bulletTransform.forward * bulletSpec.shootForce, ForceMode.Impulse);
			Destroy (bullet.gameObject, bulletSpec.bulletLifeTime);

			currentAmmoTracker.ExpendAmmo();
		}
	}

	void DisableEffects ()
	{
		gunLight.enabled = false;
	}
}
