using UnityEngine;

public class EnemyHealth_Hellephant : EnemyHealth
{
	EnemyMovement_Hellephant hellephantMovement;
	bool secondForm = false;
	HealthUI_Hellephant healthUI;

	protected override void Awake ()
	{
		base.Awake ();
		hellephantMovement = GetComponent<EnemyMovement_Hellephant> ();
		healthUI = GetComponentInChildren<HealthUI_Hellephant> ();
		healthUI.SetHealth (startingHealth);
		Debug.Log ("Health set to " + startingHealth);
	}


	protected override void Update ()
    {
		base.Update ();
    }

    public override void TakeDamage (int amount)
    {
		Debug.Log ("Damage is " + amount);

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
}
