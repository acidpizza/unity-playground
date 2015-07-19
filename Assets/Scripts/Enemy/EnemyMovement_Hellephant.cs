using UnityEngine;
using System.Collections;

public class EnemyMovement_Hellephant : MonoBehaviour
{
    Transform player;
	Rigidbody playerRigidBody;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
	Animator anim;

	bool playerInRange = false;
	bool charging = false;
	bool chargeFinalBurst = false;
	float chargeTimer = 999f;

	public float rotateSpeed = 10f;
	public float chargeTime = 1f;
	public float chargeReloadTime = 5f;

	// Line Of Sight detection
	int shootableByEnemyMask;
	Ray lineOfSightRay;
	RaycastHit playerRayHit;
	float range = 100f;

	int enemyLayer;
	int hellephantEtheralLayer;

	public SkinnedMeshRenderer hellephantRenderer;
	public Material hellephantNormalMaterial;
	public Material hellephantTransparentMaterial;

	enum EtheralState
	{
		Normal, Etheral
	};
	Color normalColor = new Color(1, 1, 1, 1);
	Color etheralColor = new Color(1, 1, 1, 0.2f);
	
    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerRigidBody = player.gameObject.GetComponent<Rigidbody> ();
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
		anim = GetComponent <Animator> ();

		enemyLayer = LayerMask.NameToLayer("Enemy");
		shootableByEnemyMask = ~enemyLayer;
		hellephantEtheralLayer = LayerMask.NameToLayer("HellephantEtheral");
    }


    void Update ()
    {
		chargeTimer += Time.deltaTime;

        if(!enemyHealth.IsDead () && !playerHealth.IsDead ())
        {
			if(charging)
			{
				ChargeToPlayer();
			}
			else
			{
				ChasePlayer();
			}
        }
        else
        {
			// Enemy or Player dead
            nav.enabled = false;

			if(playerHealth.IsDead())
			{
				anim.SetTrigger("PlayerDead");
			}
        }
    }

	void ChasePlayer()
	{
		if(playerInRange)
		{
			lineOfSightRay.origin = transform.position + new Vector3(0,0.2f,0);
			lineOfSightRay.direction = player.position - transform.position;
			
			if(HasLineOfSightToPlayer() && chargeTimer > chargeReloadTime)
			{
				// Charge to the player
				StartCharge();
			}
			else
			{
				// Player is in range but is blocked. Continue chasing to get line of sight.
			}
		}
		else
		{
			// Chase the player until player is in range
		}

		if(nav.enabled)
		{
			nav.SetDestination (player.position);
		}
	}

	void StartCharge()
	{
		chargeTimer = 0f;
		charging = true;
		nav.speed = 15;
		chargeFinalBurst = false;
		TurnEtheral ();
	}

	void EndCharge()
	{
		charging = false;
		nav.speed = 2;
		chargeFinalBurst = false;
		TurnNormal ();
		Stomp ();
	}

	void ChargeToPlayer()
	{
		if(!chargeFinalBurst)
		{
			// Initial phase of charge -> keep following player
			if(nav.remainingDistance > 4.0f)
			{
				nav.SetDestination (player.position);
			}
			else
			{
				// Near to the player -> predict movement and charge there
				chargeFinalBurst = true;
				Vector3 anticpatedLocation = player.position + playerRigidBody.velocity * 0.75f;
				nav.SetDestination (anticpatedLocation);
			}
		}
		else if(nav.remainingDistance < 1f)
		{
			EndCharge();
		}
	}

	void Stomp()
	{
		anim.SetTrigger ("Stomp");
		nav.enabled = false;
		Invoke ("EnableNav", 0.8f);
	}

	void EnableNav()
	{
		if(!enemyHealth.IsDead () && !playerHealth.IsDead ())
		{
			nav.enabled = true;
		}
	}

	void TurnEtheral()
	{
		gameObject.layer = hellephantEtheralLayer;
		hellephantRenderer.material = hellephantTransparentMaterial;
	}

	void TurnNormal()
	{
		gameObject.layer = enemyLayer;
		hellephantRenderer.material = hellephantNormalMaterial;
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
		}
	}

	bool HasLineOfSightToPlayer()
	{
		return Physics.Raycast (lineOfSightRay, out playerRayHit, range, shootableByEnemyMask) && playerRayHit.transform.tag == "Player";
	}
}
