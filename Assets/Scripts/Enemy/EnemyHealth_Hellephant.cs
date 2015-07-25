using UnityEngine;

public class EnemyHealth_Hellephant : EnemyHealth
{
	EnemyMovement_Hellephant hellephantMovement;
	bool secondForm = false;

	protected override void Awake ()
	{
		base.Awake ();
		hellephantMovement = GetComponent<EnemyMovement_Hellephant> ();
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
		secondForm = true;
	}

	public bool IsSecondForm()
	{
		return secondForm;
	}
}
