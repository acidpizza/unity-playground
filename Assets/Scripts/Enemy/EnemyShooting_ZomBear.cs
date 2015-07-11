using UnityEngine;
using System.Collections;

public class EnemyShooting_ZomBear : EnemyShooting 
{	
	Transform player;
	Rigidbody playerRigidBody;

	public ParticleSystem magicEnchantParticles;
	public ParticleSystem magicCastParticlesPrefab;

	public float minTimeBetweenCast;
	public float maxTimeBetweenCast;
	public float magicLifeTime;

	float timer;
	AudioSource magicCastAudio;

	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerRigidBody = player.gameObject.GetComponent<Rigidbody> ();
		//magicCastAudio = GetComponent<AudioSource> ();
		timer = maxTimeBetweenCast;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(IsShooting && timer >= Random.Range(minTimeBetweenCast, maxTimeBetweenCast))
		{
			Cast ();
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
		//magicCastAudio.Play ();
		magicEnchantParticles.Stop ();
		magicEnchantParticles.Play ();
	}
}
