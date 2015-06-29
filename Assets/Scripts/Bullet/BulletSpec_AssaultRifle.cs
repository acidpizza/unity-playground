using UnityEngine;
using System.Collections;

public class BulletSpec_AssaultRifle : BulletSpec
{
	public BulletSpec_AssaultRifle()
	{
		timeBetweenBurst = 1.2f;
		burstRounds = 3;
		timeBetweenBulletInBurst = 0.1f;

		damagePerShot = 40;
		bulletLifeTime = 1.5f;
		shootForce = 10f;
		shootSpread = 1f;
		
		bulletCount = 30;
		bulletName = "Assault Rifle";
	}	
}
