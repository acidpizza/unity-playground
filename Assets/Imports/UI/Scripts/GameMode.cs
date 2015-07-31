using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMode : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		if(GameConfig.gameMode == GameConfig.GameMode.Campaign)
		{
			GetComponent<Text>().text = "Campaign\n";
			if(GameConfig.difficulty == GameConfig.Difficulty.Chill)          { GetComponent<Text>().text += "Chill"; }
			else if(GameConfig.difficulty == GameConfig.Difficulty.Challenge) {	GetComponent<Text>().text += "Challenge"; }
			else if(GameConfig.difficulty == GameConfig.Difficulty.Hardcore)  { GetComponent<Text>().text += "Hardcore"; }
		}
		else if(GameConfig.gameMode == GameConfig.GameMode.Survival)
		{
			GetComponent<Text>().text = "Survival";
		}
	}
}
