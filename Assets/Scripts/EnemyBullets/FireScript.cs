using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {

	public float lifetime = 10f;
	public float timeBetweenDamage = 1f;
	public int damage = 10;

	float timer_lifetime = 0f;
	float timer_damage = 0f;
	GameObject player;
	ParticleSystem fireParticleSystem;
	PlayerHealth playerHealth;
	bool playerInRange;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();

		fireParticleSystem = GetComponent<ParticleSystem> ();
		Destroy (gameObject, lifetime + 3f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer_lifetime += Time.deltaTime;
		timer_damage += Time.deltaTime;

		if (timer_lifetime > lifetime) 
		{
			fireParticleSystem.loop = false;
		}

		if(timer_damage >= timeBetweenDamage && playerInRange)
		{
			Attack ();
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{
			playerInRange = true;
		}
	}
	
	
	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player)
		{
			playerInRange = false;
		}
	}

	void Attack ()
	{
		timer_damage = 0f;
		
		if(!playerHealth.IsDead ())
		{
			playerHealth.TakeDamage ("Meteor Flames", damage);
		}
	}
}
