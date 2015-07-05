using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour 
{
	void Start()
	{
		Cursor.visible = true;
	}

	public void OnStart()
	{
		Application.LoadLevel ("Level1");
	}
}
