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
	bool chargeFinalBurst = false;
	float chargeTimer = 999f;

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
	public ParticleSystem flameEnchant;
	public ParticleSystem dustCloud;
	Quaternion dustCloudRotation; 

	enum ActionState
	{
		Chasing, ChargePreparation, Charging
	};
	ActionState actionState = ActionState.Chasing;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerRigidBody = player.gameObject.GetComponent<Rigidbody> ();
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
		anim = GetComponent <Animator> ();

		enemyLayer = LayerMask.NameToLayer("Enemy");
		shootableByEnemyMask = ~LayerMask.GetMask ("Enemy");
		hellephantEtheralLayer = LayerMask.NameToLayer("HellephantEtheral");
		dustCloudRotation = dustCloud.transform.rotation;
    }

    void Update ()
    {
		chargeTimer += Time.deltaTime;

        if(!enemyHealth.IsDead () && !playerHealth.IsDead ())
        {
			if(actionState == ActionState.Chasing)
			{
				ChasePlayer();
			}
			else if(actionState == ActionState.Charging)
			{
				ChargeToPlayer();
			}
			else if(actionState == ActionState.ChargePreparation)
			{
				// Do nothing
			}
        }
        else
        {
			// Enemy or Player dead
            nav.enabled = false;
			dustCloud.Stop ();

			if(playerHealth.IsDead())
			{
				anim.SetTrigger("PlayerDead");
			}
        }

		dustCloud.transform.rotation = dustCloudRotation;
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
				PrepareToCharge();
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

	void PrepareToCharge()
	{
		actionState = ActionState.ChargePreparation;
		flameEnchant.Stop ();
		flameEnchant.Play ();
		Invoke ("StartCharge", 1.5f);
	}

	void StartCharge()
	{
		actionState = ActionState.Charging;
		nav.speed = 13;
		chargeFinalBurst = false;
		dustCloud.Play ();
		TurnEtheral ();
	}

	void EndCharge()
	{
		actionState = ActionState.Chasing;
		nav.speed = 2;
		chargeFinalBurst = false;
		chargeTimer = 0f;
		TurnNormal ();
		dustCloud.Stop ();
		StartStomp ();
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

	void StartStomp()
	{
		anim.SetTrigger ("Stomp");
		nav.enabled = false;
		Invoke ("EndStomp", 0.8f);
	}

	void EndStomp()
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
