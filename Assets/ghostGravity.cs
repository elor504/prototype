using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostGravity : MonoBehaviour
{
    public float gravity = -9.89f;
	bool isGrounded;
    Rigidbody2D rb;

	private void Awake()
	{

		rb = GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void FixedUpdate()
    {
		if(!isGrounded)
		rb.velocity -= new Vector2(0, gravity * Time.fixedDeltaTime);
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		isGrounded = true;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		isGrounded = false;
	}
}
