using UnityEngine;
using System.Collections;

public class BulletSpec_PulseRifle : BulletSpec
{
	public BulletSpec_PulseRifle()
	{
		timeBetweenBurst = 1.1f;
		burstRounds = 2;
		timeBetweenBulletInBurst = 0.2f;

		damagePerShot = 60;
		bulletLifeTime = 1f;
		shootForce = 12f;
		shootSpread = 0.5f;
	}	
}
