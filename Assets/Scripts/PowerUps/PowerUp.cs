using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public float rotateSpeed = 100f;
	public Rigidbody bulletStore;

	void Update () 
	{
		transform.Rotate (new Vector3 (0, 1, 0) * rotateSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerShooting playerShooting = other.GetComponentInChildren<PlayerShooting> ();
		if(playerShooting != null)
		{
			playerShooting.ChangeWeapon(bulletStore);
			Destroy(transform.parent.gameObject);
		}
	}
}
