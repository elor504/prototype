using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public string playerTag;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == playerTag)
		{
			Debug.Log("Touchy");
			GameManager.getInstance.ResetGame();

		}
	}

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == playerTag)
		{
			Debug.Log("Touchy");
			GameManager.getInstance.ResetGame();

		}
	}


}
