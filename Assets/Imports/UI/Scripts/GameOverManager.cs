using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    Animator anim;
	public Text gameOverText;

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

	public void Win()
	{
		gameOverText.text = "YOU WIN!!!";
		GameOver ();
	}
}
