using UnityEngine;
using System.Collections;

public class BulletSpec_AssaultRifle : BulletSpec
{
	public BulletSpec_AssaultRifle()
	{
		timeBetweenBurst = 0.6f;
		burstRounds = 3;
		timeBetweenBulletInBurst = 0.1f;

		damagePerShot = 40;
		bulletLifeTime = 1.5f;
		shootForce = 10f;
		shootSpread = 1f;
	}	
}
