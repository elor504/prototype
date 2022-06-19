using UnityEngine;

public class PickUpInteraction : ObjectInteraction
{
	public GameObject pickupSprite;
	public DoorInteraction door;

	public override void Awake()
	{
		base.Awake();
		InitPickUp();
	}
	public void InitPickUp()
	{
		isInteracted = false;
		pickupSprite.SetActive(true);
	}
	public override void OnInteraction()
	{
		isInteracted = true;
		pickupSprite.SetActive(false);
		door.OnGettingKey();
	}

	public override void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);
	}

}
