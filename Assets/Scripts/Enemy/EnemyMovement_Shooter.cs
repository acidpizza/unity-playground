using UnityEngine;
using System.Collections;

public class EnemyMovement_Shooter : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
	Animator anim;

	bool playerInRange;
	public float rotateSpeed = 10f;

	// Line Of Sight detection
	int shootableByEnemyMask;
	Ray lineOfSightRay;
	RaycastHit playerRayHit;
	float range = 100f;
	//	LineRenderer gunLine;

	public EnemyShooting enemyShooting;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
		anim = GetComponent <Animator> ();

		shootableByEnemyMask = LayerMask.GetMask ("ShootableByEnemy");
//		gunLine = GetComponent<LineRenderer> ();
    }


    void Update ()
    {
        if(!enemyHealth.IsDead () && !playerHealth.IsDead ())
        {
			if(playerInRange)
			{
				lineOfSightRay.origin = transform.position + new Vector3(0,0.2f,0);
				lineOfSightRay.direction = player.position - transform.position;

            	if(HasLineOfSightToPlayer())
				{
					nav.enabled = false;
					if(TurnAndIsFacingPlayer())
					{
						anim.SetBool("PlayerInSight", true);
						enemyShooting.IsShooting = true;
					}
					else
					{
						anim.SetBool("PlayerInSight", false);
						enemyShooting.IsShooting = false;
					}
				}
				else
				{
					// Player is in range but is blocked. Continue chasing to get line of sight.
					anim.SetBool("PlayerInSight", false);
					enemyShooting.IsShooting = false;

					nav.enabled = true;
					nav.SetDestination (player.position);
				}
			}
			else
			{
				// Chase the player until player is in range
				anim.SetBool("PlayerInSight", false);
				enemyShooting.IsShooting = false;

				nav.enabled = true;
				nav.SetDestination (player.position);
			}
        }
        else
        {
			// Enemy of Player dead
            nav.enabled = false;
			if(playerHealth.IsDead())
			{
				anim.SetTrigger("PlayerDead");
				enemyShooting.IsShooting = false;
			}
        }
    }

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player.gameObject)
		{
			playerInRange = true;
		}
	}
	
	
	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player.gameObject)
		{
			playerInRange = false;
//			gunLine.enabled = false;
		}
	}

	bool HasLineOfSightToPlayer()
	{
		return Physics.Raycast (lineOfSightRay, out playerRayHit, range, shootableByEnemyMask) && playerRayHit.transform.tag == "Player";
		/*
		if(Physics.Raycast (lineOfSightRay, out playerRayHit, range, shootableByEnemyMask))
		{
			gunLine.enabled = true;
			gunLine.SetPosition (0, transform.position + new Vector3(0,0.2f,0));
			gunLine.SetPosition (1, playerRayHit.point);

			return playerRayHit.transform.tag == "Player";
		}
		gunLine.enabled = false;
		return false;
		*/
	}

	bool TurnAndIsFacingPlayer()
	{
		Quaternion wantedRot=Quaternion.LookRotation(lineOfSightRay.direction);
		transform.rotation=Quaternion.Slerp(transform.rotation, wantedRot, rotateSpeed*Time.deltaTime);
		return (Quaternion.Angle (transform.rotation, wantedRot) < 5);
	}
}
