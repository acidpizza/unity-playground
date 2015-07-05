using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

	public void GameOver()
	{
		anim.SetTrigger ("GameOver");
		Invoke ("RestartApplication", 4f);
	}

	void RestartApplication()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}
