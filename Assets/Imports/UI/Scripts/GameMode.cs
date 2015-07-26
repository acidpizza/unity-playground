using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMode : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		if(GameConfig.setting == GameConfig.Setting.Campaign)
		{
			GetComponent<Text>().text = "Campaign";
		}
		else if(GameConfig.setting == GameConfig.Setting.Survival)
		{
			GetComponent<Text>().text = "Survival";
		}
	}
}
