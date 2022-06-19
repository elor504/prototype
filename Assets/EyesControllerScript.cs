using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesControllerScript : MonoBehaviour
{
    [SerializeField] Transform pupilTranform;
    [SerializeField] Transform eyeTranform;

    [SerializeField] Transform ghosyBoyTranform;
    public float eyeOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = ghosyBoyTranform.position - eyeTranform.position;


    }

}
