using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererManager : MonoBehaviour
{
    public RopePhysic rope;

    public LineRenderer lineRenderer;
    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = rope.ropePositions.Count;

        //Vector2[] ropePositions = rope.ropePositions.ToArray();

        Vector3[] ropePositions = new Vector3[rope.ropePositions.Count];
		for (int i = 0; i < ropePositions.Length; i++)
		{
            ropePositions[i] = rope.ropePositions[i];

        }
        lineRenderer.SetPositions(ropePositions);

    }




}
