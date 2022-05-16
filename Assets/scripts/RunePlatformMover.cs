using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunePlatformMover : Rune
{
	public List<MovingPlatform> movingPlatfroms;
	public override void InitRune()
	{
		base.InitRune();
	
	}

	void MovePlatforms(){
		for (int i = 0; i < movingPlatfroms.Count; i++)
		{
			movingPlatfroms[i].MovePlatform();
		}
	}

	public override void UseRune()
	{
		base.UseRune();
		MovePlatforms();


	}


	

}
