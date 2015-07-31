using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour 
{
	public Toggle chillToggle, challengeToggle, hardcoreToggle;

	void Start()
	{
		Cursor.visible = true;
	}

	void Awake()
	{
		if(GameConfig.difficulty == GameConfig.Difficulty.Chill)          { chillToggle.isOn = true; }
		else if(GameConfig.difficulty == GameConfig.Difficulty.Challenge) {	challengeToggle.isOn = true; }
		else if(GameConfig.difficulty == GameConfig.Difficulty.Hardcore)  { hardcoreToggle.isOn = true; }
	}

	public void OnCampaign()
	{
		GameConfig.gameMode = GameConfig.GameMode.Campaign;
		Application.LoadLevel ("Level1");
	}

	public void OnSurvival()
	{
		GameConfig.gameMode = GameConfig.GameMode.Survival;
		Application.LoadLevel ("Level1");
	}

	public void OnDifficultyChill(bool active)
	{
		if(active) {GameConfig.difficulty = GameConfig.Difficulty.Chill; }
	}

	public void OnDifficultyChallenge(bool active)
	{
		if(active) {GameConfig.difficulty = GameConfig.Difficulty.Challenge; }
	}

	public void OnDifficultyHardcore(bool active)
	{
		if(active) {GameConfig.difficulty = GameConfig.Difficulty.Hardcore; }
	}
}
