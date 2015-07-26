using UnityEngine;

public class EnemyHealth_Hellephant : EnemyHealth
{
	EnemyMovement_Hellephant hellephantMovement;
	bool secondForm = false;
	HealthUI_Hellephant healthUI;
	GameOverManager gameOverManager;

	protected override void Awake ()
	{
		base.Awake ();
		hellephantMovement = GetComponent<EnemyMovement_Hellephant> ();
		healthUI = GetComponentInChildren<HealthUI_Hellephant> ();
		healthUI.SetHealth (startingHealth);
	
		gameOverManager = FindObjectOfType(typeof(GameOverManager)) as GameOverManager;
	}


	protected override void Update ()
    {
		base.Update ();
    }

    public override void TakeDamage (int amount)
    {
        if(!isDead)
		{
	        enemyAudio.Play ();

	        currentHealth -= amount;
			healthUI.TakeDamage(amount);

	        if(currentHealth <= 0)
	        {
	            Death ();
	        }
			else if(!secondForm && currentHealth <= 4f/10f * startingHealth)
			{
				TakeSecondForm();
			}
		}
    }

	void TakeSecondForm()
	{
		hellephantMovement.TakeSecondForm ();
		gameManager.ActivateBossSecondForm ();
		healthUI.SetLowHealthStatus ();
		secondForm = true;
	}

	public bool IsSecondForm()
	{
		return secondForm;
	}

	protected override void Death()
	{
		hellephantMovement.RevertToFirstForm ();
		Invoke ("KillAllEnemies", 3f);
		gameManager.WinGame ();
		base.Death ();
	}

	public override void StartSinking ()
	{
		GetComponent <NavMeshAgent> ().enabled = false;
		GetComponent <Rigidbody> ().isKinematic = true;
		//isSinking = true;
		gameManager.AddScore(scoreValue, enemyName, transform);
		//Destroy (gameObject, 2f);
	}

	void KillAllEnemies()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies)
		{
			enemy.GetComponent<EnemyHealth>().TakeDamage(99999);
		}
		Invoke ("Win", 2f);
	}

	void Win()
	{
		gameOverManager.Win ();
	}
}
