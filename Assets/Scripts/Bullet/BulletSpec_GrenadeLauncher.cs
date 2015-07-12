using UnityEngine;
using System.Collections;

public class BulletSpec_GrenadeLauncher : BulletSpec
{
	public Transform grenadeExplodePrefab;

	float timer;
	bool exploded = false;

	public BulletSpec_GrenadeLauncher()
	{
		timeBetweenBurst = 4f;
		burstRounds = 0;
		timeBetweenBulletInBurst = 0f;

		damagePerShot = 0;
		bulletLifeTime = 2f;
		shootForce = 10f;
		shootSpread = 0f;
		
		bulletCount = 10;
		bulletName = "Grenade Launcher";

		isProjectile = true;
		liftForce = 3f;
		forwardTorque = 3f;
	}	

	void Update()
	{
		timer += Time.deltaTime;
		if(!exploded && timer >= bulletLifeTime)
		{
			exploded = true;

			foreach( Renderer renderer in GetComponentsInChildren<MeshRenderer>())
			{
				renderer.enabled = false;
			}
			GetComponent<Collider>().enabled = false;

			Instantiate (grenadeExplodePrefab, transform.position, transform.rotation * grenadeExplodePrefab.transform.rotation);

			Destroy(gameObject);
		}
	}

	protected override void OnCollisionEnter(Collision other) 
	{

	}
}
