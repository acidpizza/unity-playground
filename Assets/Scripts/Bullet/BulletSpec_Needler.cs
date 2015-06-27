using UnityEngine;
using System.Collections;

public class BulletSpec_Needler : BulletSpec
{
	public BulletSpec_Needler()
	{
		timeBetweenBullets = 0.4f;
		burstRounds = 3;
		burstTime = 1f;

		damagePerShot = 20;
		bulletLifeTime = 3f;
		shootForce = 10f;
		shootSpread = 2f;
	}	
}
