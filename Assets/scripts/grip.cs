using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grip : MonoBehaviour
{
	[SerializeField] float[] breakableAngle;
    [SerializeField] bool isBeingUsed;
	[SerializeField] bool isRopeOnRight;
	public bool getIsBeingUsed => isBeingUsed;
	public bool getIsRopeOnRight => isRopeOnRight;
    public void Attach(bool _isAttached,bool _isOnRight)
	{
		isBeingUsed = _isAttached;
		isRopeOnRight = _isOnRight;
	}

	public bool IsOnBreakableAngleRange(float _angle)
	{
		//Debug.Log(" Angle: " + _angle);


		if (breakableAngle[0] > _angle && breakableAngle[1] < _angle)
		{
		
			return true;
		}
		return false;
	}
	public Vector2 GetGripPosition => this.transform.position;

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.z;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(this.transform.position,new Vector2(0.05f,0.05f));

		Vector3 viewAngleA = DirFromAngle(breakableAngle[0] - 90, false);
		Vector3 viewAngleB = DirFromAngle((breakableAngle[1] - 90), false);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(this.transform.position, this.transform.position + viewAngleA * 1);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(this.transform.position, this.transform.position + viewAngleB * 1);
	}

}
