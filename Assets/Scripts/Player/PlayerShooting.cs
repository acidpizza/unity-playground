using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	
    float timer;
    Ray shootRay;
	Ray shootRay_check;
	Vector3 playerMiddlePosition;
	RaycastHit shootHit;
    int shootableMask;
	bool hit = false;
	float gunTipOverExtension = 0.7f;

	public ParticleSystem gunParticles;
	public ParticleSystem hitParticles;

	LineRenderer gunLine;
	AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        //gunParticles = GetComponent<ParticleSystem> ();
		//hitParticles = GetComponentInChildren <ParticleSystem> ();

		gunLine = GetComponent <LineRenderer> ();
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
        gunLine.enabled = false;
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
			gunLine.enabled = false;
			hit = true;
		}
        else 
		{
			gunLine.enabled = true;
			gunLine.SetPosition (0, transform.position);

			// check if anything was hit
			shootRay.origin = transform.position + (transform.forward * -1);
			shootRay.direction = transform.forward; // check forward
			if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
	        {
				// Something was hit -> truncate laser length and play hit particles
	            gunLine.SetPosition (1, shootHit.point);
				hit = true;
	        }
	        else
	        {
				// Nothing was hit -> shoot to maximum range
				gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
	        }
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
