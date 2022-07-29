using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatLineRendererScript : MonoBehaviour
{
    [SerializeField] private Transform movePlatLineRend1;
    [SerializeField] private Transform movePlatLineRend2;
    [SerializeField] private LineRenderer LineRend;

    // Start is called before the first frame update
    void Start()
    {
        movePlatLineRend1.localPosition = new Vector3(movePlatLineRend1.localPosition.x, movePlatLineRend1.localPosition.y,0);
        movePlatLineRend2.localPosition = new Vector3(movePlatLineRend2.localPosition.x, movePlatLineRend2.localPosition.y,0);


        LineRend.SetPosition(0, movePlatLineRend1.localPosition);
        LineRend.SetPosition(1, movePlatLineRend2.localPosition);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
