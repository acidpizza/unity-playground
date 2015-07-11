using UnityEngine;
using System.Collections;

public class EnemyMagicCast : MonoBehaviour {

	public float slowRadius;
	public float magicSlowDelay;
	public float explosionRadius;
	public float explosionDamageDelay;
	public float explosionEndTime;
	public int explosionDamage;

	Transform player;
	PlayerHealth playerHealth;
	PlayerMovement playerMovement;

	bool isSlowActive = false;
	bool isExplosionDamageDealt = false;
	float timer;

	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth = player.GetComponent <PlayerHealth> ();
		playerMovement = player.GetComponent <PlayerMovement> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		// Slow
		if(!isSlowActive && timer >= magicSlowDelay)
		{
			float distanceToPlayer = (transform.position - player.position).magnitude;
			if(distanceToPlayer < slowRadius)
			{
				isSlowActive = true;
				playerMovement.MagicSlowHit();
			}
		}

		// Damage
		if(!isExplosionDamageDealt && timer >= explosionDamageDelay && timer <= explosionEndTime)
		{
			float distanceToPlayer = (transform.position - player.position).magnitude;
			if(distanceToPlayer < explosionRadius)
			{
				isExplosionDamageDealt = true;
				playerHealth.TakeDamage(explosionDamage);
			}
		}
	}

	void OnDisable()
	{
		if(isSlowActive)
		{
			playerMovement.MagicSlowRelease();
			isSlowActive = false;
		}
	}
}
