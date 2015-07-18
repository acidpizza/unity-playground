using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public AudioClip deathClip;
	public HealthUI healthUI;
	public GameOverManager gameOverManager;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    
	bool isDead;
	int currentHealth;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
		healthUI.SetHealth(startingHealth);
    }

	public bool IsDead()
	{
		return isDead;
	}

	public void TakeDamage (string enemyName, int value)
    {
		if(!isDead)
		{
	        currentHealth -= value;
			healthUI.TakeDamage (value);

	        playerAudio.Play ();

	        if(currentHealth <= 0 && !isDead)
	        {
	            Death ();
	        }

			BulletTracker.EnemyDamage (enemyName, value);
		}
    }

	public void GainHealth(int value)
	{
		if (currentHealth + value > 100)
		{
			currentHealth = 100;
		}
		else
		{
			currentHealth += value;
		}
		healthUI.SetHealth (currentHealth);
	}

    void Death ()
    {
        isDead = true;

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;

		Invoke ("TriggerGameOverUI", 3f);
    }

	void TriggerGameOverUI()
	{
		gameOverManager.GameOver ();
	}
}
