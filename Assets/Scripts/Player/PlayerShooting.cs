using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour 
{
	public class AmmoTracker
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
			}
		}

		public Rigidbody bulletPrefab;
		public BulletSpec bulletSpec;
		public int bulletCount;
		public bool unlimitedAmmo;
	};

	public AmmoUI ammoUI;
	public Rigidbody bulletPrefab;
	BulletSpec bulletSpec;

	List<AmmoTracker> ammoStore = new List<AmmoTracker> ();
	int ammoStoreIndex = 0;
	AmmoTracker currentAmmoTracker;

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

		if(Input.GetButtonDown ("ChangeWeaponRight"))
		{
			ChangeWeapon (true);
		}
		else if(Input.GetButtonDown ("ChangeWeaponLeft"))
		{
			ChangeWeapon (false);
		}

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
				}
				else if(currentBurstCount >= bulletSpec.burstRounds)
				{
					// Hit burst limit -> need to wait for new burst
					if(timer >= bulletSpec.timeBetweenBurst)
					{
						currentBurstCount = 1;
						Shoot ();
					}
				}
			}
			else
			{
				// Burst disabled
				if(timer >= bulletSpec.timeBetweenBurst)
				{
					Shoot ();
				}
			}
		}
	}

	public void CollectWeapon(Rigidbody newBulletPrefab)
	{
		for(int i = 0; i < ammoStore.Count; i++)
		{
			AmmoTracker ammoTracker = ammoStore[i];
			if(ammoTracker.AddAmmo(newBulletPrefab))
			{
				if(ammoStoreIndex == i)
				{
					ammoUI.UpdateAmmo(ammoTracker); // Only update UI if currently showing this gun
				}
				return;
			}
		}

		{
			// Cannot find, add new AmmoTracker
			ammoStore.Add(new AmmoTracker(newBulletPrefab));
			AmmoTracker ammoTracker = ammoStore[ammoStore.Count - 1];

			// Update currentAmmoTracker and UI if this is the 1st weapon collected
			if(ammoStoreIndex == ammoStore.Count - 1)
			{
				currentAmmoTracker = ammoTracker;
				bulletPrefab = ammoTracker.bulletPrefab;
				bulletSpec = ammoTracker.bulletSpec;
				ammoUI.UpdateWeapon(currentAmmoTracker);
				gunAudio.clip = bulletSpec.shootAudio;
			}
		}
	}

	// changeWeapon_input: Positive = change up. Negative = change down.
	public void ChangeWeapon(bool changeWeapon_input)
	{
		if (changeWeapon_input) 
		{
			ammoStoreIndex = (ammoStoreIndex + 1) % ammoStore.Count;
		}
		else
		{
			if(ammoStoreIndex == 0)
			{
				ammoStoreIndex = ammoStore.Count - 1;
			}
			else
			{
				ammoStoreIndex--;
			}
		}

		currentAmmoTracker = ammoStore [ammoStoreIndex];
		bulletPrefab = currentAmmoTracker.bulletPrefab;
		bulletSpec = currentAmmoTracker.bulletSpec;
		ammoUI.UpdateWeapon(currentAmmoTracker);
		gunAudio.clip = bulletSpec.shootAudio;
	}

	void Shoot ()
	{
		if(currentAmmoTracker.AmmoAvailable())
		{
			timer = 0f;
			EnableEffects();

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
			ammoUI.UpdateAmmo(currentAmmoTracker);

			Invoke ("DisableEffects", effectsDisplayTime);
		}
	}

	void EnableEffects()
	{
		gunAudio.Play ();
		gunLight.enabled = true;
		gunFlareParticles.Stop ();
		gunFlareParticles.Play ();
	}
	
	void DisableEffects ()
	{
		gunLight.enabled = false;
	}
}
