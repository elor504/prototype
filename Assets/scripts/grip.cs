using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grip : MonoBehaviour
{

	public bool isMouseOnGrip;


	
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Mouse")
		{
			isMouseOnGrip = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Mouse")
		{
			isMouseOnGrip = false;
		}
	}
}
