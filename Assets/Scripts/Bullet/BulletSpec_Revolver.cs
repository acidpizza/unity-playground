using UnityEngine;
using System.Collections;

public class BulletSpec_Revolver : BulletSpec
{
	public BulletSpec_Revolver()
	{
		timeBetweenBurst = 1f;
		burstRounds = 6;
		timeBetweenBulletInBurst = 0.4f;

		damagePerShot = 20;
		bulletLifeTime = 0.8f;
		shootForce = 6f;
		shootSpread = 2f;

		bulletCount = 0;
		bulletName = "Revolver";
	}	
}
