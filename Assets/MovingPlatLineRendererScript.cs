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
        movePlatLineRend1.position = new Vector3(movePlatLineRend1.position.x, movePlatLineRend1.position.y,0);
        movePlatLineRend2.position = new Vector3(movePlatLineRend2.position.x, movePlatLineRend2.position.y,0);


        LineRend.SetPosition(0, movePlatLineRend1.position);
        LineRend.SetPosition(1, movePlatLineRend2.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
