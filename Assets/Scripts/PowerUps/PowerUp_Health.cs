using UnityEngine;
using System.Collections;

public class PowerUp_Health : MonoBehaviour {

	public float rotateSpeed = 100f;
	public int healthGain = 10;
	AudioSource gainHealthAudio;

	void Awake()
	{
		gainHealthAudio = GetComponent<AudioSource> ();
	}

	void Update () 
	{
		transform.Rotate (new Vector3 (0, 0, 1) * rotateSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth> ();
		if(playerHealth != null)
		{
			gainHealthAudio.Play();
			playerHealth.GainHealth(healthGain);
			GetComponent<Renderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
			Destroy(gameObject, 2.0f);
		}
	}
}
