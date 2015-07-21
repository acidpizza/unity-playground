using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    public PlayerHealth playerHealth_;
    public GameObject enemy_;
    public float spawnTime_ = 3f;
	public int burstSpawn_ = 1;
    public Transform[] spawnPoints_;
	AudioSource spawnAudioSource;

	float timer_ = 999f;
	HashSet<int> spawnPointIndices_ = new HashSet<int>();
	int spawnPointIndex_ = 0;

	void Awake()
	{
		spawnAudioSource = GetComponent<AudioSource> ();
		if (burstSpawn_ > spawnPoints_.Length) 
		{
			Debug.Log ("Warning: More burst spawns than spawn points available!");
		}
	}

	void Update()
	{
		timer_ += Time.deltaTime;

		if(timer_ >= spawnTime_)
		{
			Spawn ();
			timer_ = 0;
			if(spawnAudioSource != null)
			{
				spawnAudioSource.Play ();
			}
		}
	}


    void Spawn ()
    {
        if(playerHealth_.IsDead ())
        {
            return;
        }

		spawnPointIndices_.Clear ();

		for(int i=0; i<burstSpawn_; i++)
		{
			do
			{
				spawnPointIndex_ = Random.Range (0, spawnPoints_.Length);
			}while(!spawnPointIndices_.Add(spawnPointIndex_));
		}

		foreach(int spawnPointIndex in spawnPointIndices_)
		{
        	Instantiate (enemy_, spawnPoints_[spawnPointIndex].position, enemy_.transform.rotation);
		}
    }
}
