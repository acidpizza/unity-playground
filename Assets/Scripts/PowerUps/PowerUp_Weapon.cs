using UnityEngine;
using System.Collections;

public class PowerUp_Weapon : MonoBehaviour {

	public float rotateSpeed = 100f;
	public Rigidbody bulletStore;
	AudioSource reloadAudio;

	void Awake()
	{
		reloadAudio = GetComponent<AudioSource> ();
	}

	void Update () 
	{
		transform.Rotate (new Vector3 (0, 1, 0) * rotateSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerShooting playerShooting = other.GetComponentInChildren<PlayerShooting> ();
		if(playerShooting != null)
		{
			reloadAudio.Play();
			playerShooting.CollectWeapon(bulletStore);
			foreach( Renderer renderer in transform.parent.GetComponentsInChildren<Renderer>())
			{
				renderer.enabled = false;
			}

			GetComponent<Collider>().enabled = false;
			Destroy(transform.parent.gameObject, 1.0f);
		}
	}
}
