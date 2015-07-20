using UnityEngine;
using System.Collections;

public class MeteorStrike : MonoBehaviour {

	public Transform firePrefab;
	public float strikeRadius = 1.2f;
	public int damage = 15;

	ParticleSystem meteorParticleSystem;
	ParticleCollisionEvent[] collisionEvents;

	Transform player;
	PlayerHealth playerHealth;

	void Awake()
	{
		meteorParticleSystem = GetComponent<ParticleSystem> ();
		collisionEvents = new ParticleCollisionEvent[16];

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth = player.GetComponent <PlayerHealth> ();
	}

	// Update is called once per frame
	void Update () 
	{
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = meteorParticleSystem.GetCollisionEvents (other, collisionEvents);

		if(numCollisionEvents > 0)
		{
			Instantiate(firePrefab.gameObject, collisionEvents[0].intersection, firePrefab.rotation);

			float distanceToPlayer = (collisionEvents[0].intersection - player.position).magnitude;
			if(distanceToPlayer < strikeRadius)
			{
				playerHealth.TakeDamage("Meteor Strike", damage);
			}
		}
	}

}
