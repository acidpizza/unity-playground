using UnityEngine;
using System.Collections;

public class PlayerShooting_Bullet : MonoBehaviour 
{
	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float bulletLifeTime = 3f;
	public float shootForce = 100f;
	
	public ParticleSystem gunParticles;
	public ParticleSystem hitParticles;
	public Rigidbody bulletPrefab;
	
	float timer;
	Ray shootRay_check;
	Vector3 playerMiddlePosition;
	RaycastHit shootHit;
	int shootableMask;
	bool hit = false;
	float gunTipOverExtension = 0.7f;

	AudioSource gunAudio;
	Light gunLight;
	float effectsDisplayTime = 0.2f;
	
	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}
	
	
	void Update ()
	{
		timer += Time.deltaTime;
		
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
		{
			Shoot ();
		}
		
		if(timer >= timeBetweenBullets * effectsDisplayTime)
		{
			DisableEffects ();
		}
	}
	
	
	public void DisableEffects ()
	{
		gunLight.enabled = false;
	}
	
	
	void Shoot ()
	{
		timer = 0f;
		gunAudio.Play ();
		gunLight.enabled = true;
		gunParticles.Stop ();
		gunParticles.Play ();
		
		hit = false;
		//check if player has cut through an obstacle
		playerMiddlePosition = transform.position;
		playerMiddlePosition.z -= gunTipOverExtension; // distance from player middle to gun barrel is 0.7f
		shootRay_check.origin = playerMiddlePosition;
		shootRay_check.direction = transform.forward;
		if(Physics.Raycast (shootRay_check, out shootHit, gunTipOverExtension, shootableMask))
		{
			// Player has cut through an obstacle. Don't shoot past it.
			hit = true;
		}
		else 
		{

			Rigidbody bullet = Instantiate (bulletPrefab, transform.position, transform.rotation * bulletPrefab.transform.rotation) as Rigidbody;
			bullet.AddForce(transform.forward * shootForce, ForceMode.Impulse);
			Destroy (bullet.gameObject, bulletLifeTime);
		}
		
		if(hit)
		{
			hitParticles.transform.position = shootHit.point;
			hitParticles.Play();
			/*
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
	        if(enemyHealth != null)
	        {
	            enemyHealth.TakeDamage (damagePerShot, shootHit.point);
	        }
			*/
		}
	}
}
