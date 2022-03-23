using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
	public bool isInteracted;

	public virtual void Awake()
	{
		isInteracted = false;
	}


	public virtual void OnInteraction(){}



	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isInteracted && collision.tag == "Player")
		{
			this.OnInteraction();
		}
	}

}
