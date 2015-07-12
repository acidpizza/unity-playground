using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public PlayerHealth playerHealth_;
    public GameObject enemy_;
    public float spawnTime_ = 3f;
    public Transform[] spawnPoints_;

	float timer = 0f;


	void Update()
	{
		timer += Time.deltaTime;

		if(timer >= spawnTime_)
		{
			Spawn ();
			timer = 0;
		}
	}


    void Spawn ()
    {
        if(playerHealth_.IsDead ())
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints_.Length);

        Instantiate (enemy_, spawnPoints_[spawnPointIndex].position, spawnPoints_[spawnPointIndex].rotation);
    }
}
