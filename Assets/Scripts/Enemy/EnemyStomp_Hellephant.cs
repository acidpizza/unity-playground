using UnityEngine;
using System.Collections;

public class EnemyStomp_Hellephant : MonoBehaviour 
{	
	Transform player;
	Rigidbody playerRigidBody;

	public ParticleSystem magicEnchantParticles;
	public ParticleSystem magicCastParticlesPrefab;

	public float minTimeBetweenCast;
	public float maxTimeBetweenCast;
	public float magicLifeTime;

	public EnemyHealth enemyHealth;

	float timer;

	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerRigidBody = player.gameObject.GetComponent<Rigidbody> ();
		timer = maxTimeBetweenCast;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(timer >= Random.Range(minTimeBetweenCast, maxTimeBetweenCast))
		{
			Cast ();
		}

		if(enemyHealth.IsDead())
		{
			DisableEffects();
		}
	}

	void Cast()
	{
		timer = 0f;
		EnableEffects ();

		Vector3 anticpatedLocation = player.position + playerRigidBody.velocity * 1.2f;
		ParticleSystem magicCastParticles = Instantiate (magicCastParticlesPrefab, anticpatedLocation, magicCastParticlesPrefab.transform.rotation) as ParticleSystem;
		magicCastParticles.Stop ();
		magicCastParticles.Play ();
		Destroy (magicCastParticles.gameObject, magicLifeTime);
	}

	void EnableEffects()
	{
		magicEnchantParticles.Stop ();
		magicEnchantParticles.Play ();
	}

	void DisableEffects()
	{
		magicEnchantParticles.Stop ();
	}
}
