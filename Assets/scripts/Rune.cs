using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : grip
{
	[SerializeField]private bool isTurnedOff;
	public bool getIsTurnedOff => isTurnedOff;
	public void InitRune()
	{
		isTurnedOff = false;
	}


	public void DisableRune()
	{
		isTurnedOff = true;
	}
}
