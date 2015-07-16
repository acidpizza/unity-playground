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
		Cursor.visible = true;
	}

	public void RestartApplication()
	{
		Application.LoadLevel ("StartMenu");
	}
}
