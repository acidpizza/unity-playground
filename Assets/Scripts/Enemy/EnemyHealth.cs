using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int scoreValue = 1;
    public AudioClip deathClip;
	public string enemyName = "ZomBunny";

	protected GameManager gameManager;
	protected bool isDead;
	protected AudioSource enemyAudio;
	protected int currentHealth;

    Animator anim;
    CapsuleCollider capsuleCollider;
    protected bool isSinking;
	float sinkSpeed = 1f;


	protected virtual void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
		gameManager = GameObject.Find ("Game").GetComponent<GameManager> ();

        currentHealth = startingHealth;
    }


    protected virtual void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


	public virtual void TakeDamage (int amount)
    {
        if(!isDead)
		{
	        enemyAudio.Play ();

	        currentHealth -= amount;
	        
	        if(currentHealth <= 0)
	        {
	            Death ();
	        }
		}
    }

	public bool IsDead()
	{
		return isDead;
	}


    protected virtual void Death ()
    {
		gameManager.AddScore(scoreValue, enemyName, transform);

        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public virtual void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        Destroy (gameObject, 2f);
    }
}
