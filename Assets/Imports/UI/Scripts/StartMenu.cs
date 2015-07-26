using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour 
{
	void Start()
	{
		Cursor.visible = true;
	}

	public void OnCampaign()
	{
		GameConfig.setting = GameConfig.Setting.Campaign;
		Application.LoadLevel ("Level1");
	}

	public void OnSurvival()
	{
		GameConfig.setting = GameConfig.Setting.Survival;
		Application.LoadLevel ("Level1");
	}
}
