using UnityEngine;

public class GhostAnimator : MonoBehaviour
{
	Animator anim;
	SpriteRenderer spriteRendrer;
	private void Awake()
	{
		anim = GetComponent<Animator>();
		spriteRendrer = GetComponentInChildren<SpriteRenderer>();
		//SetAnimBool("Idle", true);
		anim.SetBool("Idle", true);
	}

	public void RotateSpriteTowardDirection(Vector2 _lookPos)
	{
		//Vector2 direction = (_lookPos - (Vector2)this.transform.position).normalized;
		//float angle = SignedAngleBetween(_lookPos, this.transform.position);


		//if (direction.x >= 0)
		//{
		//	Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.right);
		//	spriteRendrer.transform.eulerAngles = rotation.eulerAngles;

		//	//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
		//	//going right
		//	Debug.Log("Look right");
		//}
		//else if (direction.x < 0)
		//{
		//	Debug.Log("Angle:" + angle);
		//	Debug.Log("Look left");
		//	Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.right);
		//	spriteRendrer.transform.eulerAngles = rotation.eulerAngles;

		//	//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
		//	//going left
		//}



	}

	float SignedAngleBetween(Vector3 a, Vector3 b)
	{
		// Angle between -1 and +1
		float fAngle = Vector3.Cross(a.normalized, b.normalized).y;

		// Convert to -180 to +180 degrees
		fAngle *= 180.0f;

		return (fAngle);
	}

	public void RotateToNormal()
	{
		//Debug.Log("Rotate to normal");
		//spriteRendrer.transform.eulerAngles = Vector3.zero;
	}
	public void SetAnimBool(string _boolMame, bool _bool)
	{
		//anim.SetBool(_boolMame, _bool);
	}

}
