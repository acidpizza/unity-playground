using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour {

	public Text weaponText;
	public Text ammoCountText;

	public void UpdateWeapon(PlayerShooting.AmmoTracker ammoTracker)
	{
		weaponText.text = ammoTracker.bulletSpec.bulletName + ":";
		UpdateAmmo (ammoTracker);
	}

	public void UpdateAmmo(PlayerShooting.AmmoTracker ammoTracker)
	{
		if(!ammoTracker.unlimitedAmmo)
		{
			ammoCountText.text = ammoTracker.bulletCount.ToString();
		}
		else
		{
			ammoCountText.text = "∞";
		}
	}

}
