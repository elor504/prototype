using System.Collections.Generic;
using UnityEngine;

public class RopeRendererTest : MonoBehaviour
{
    public Transform player;

    public LineRenderer rope;
    public LayerMask collMask;

    public List<Vector3> ropePositions { get; set; } = new List<Vector3>();

    private void Awake() => AddPosToRope(Vector3.zero);

    private void Update()
    {
        UpdateRopePositions();
        LastSegmentGoToPlayerPos();

        DetectCollisionEnter();
        if (ropePositions.Count > 2) DetectCollisionExits();
    }

    private void DetectCollisionEnter()
    {
        RaycastHit2D hit;
        if (Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 2), collMask))
        {
            hit = Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 2), collMask);
            if (System.Math.Abs(Vector3.Distance(rope.GetPosition(ropePositions.Count - 2), hit.point)) > 1)
            {
                ropePositions.RemoveAt(ropePositions.Count - 1);
                AddPosToRope(hit.point);
            }
        }
    }

    private void DetectCollisionExits()
    {
        RaycastHit hit;
        if (!Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 3), collMask))
        {
            ropePositions.RemoveAt(ropePositions.Count - 2);
        }
    }

    private void AddPosToRope(Vector3 _pos)
    {
        ropePositions.Add(_pos);
        ropePositions.Add(player.position); //Always the last pos must be the player
    }

    private void UpdateRopePositions()
    {
        rope.positionCount = ropePositions.Count;
        rope.SetPositions(ropePositions.ToArray());
    }

    private void LastSegmentGoToPlayerPos() => rope.SetPosition(rope.positionCount - 1, player.position);
}