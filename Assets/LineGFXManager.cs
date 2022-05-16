using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGFXManager : MonoBehaviour
{
    [SerializeField] LineRenderer mainLine;
    [SerializeField] LineRenderer prefabGFXLine;

    private int lineDots = 2;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Instantiate(prefabGFXLine);
        prefabGFXLine.SetPosition(0, mainLine.GetPosition(0));
        prefabGFXLine.SetPosition(1, mainLine.GetPosition(1));

    }

    // Update is called once per frame
    void Update()
    {
        if (mainLine.positionCount == 2)
        {
            prefabGFXLine.SetPosition(0, mainLine.GetPosition(0));
            prefabGFXLine.SetPosition(1, mainLine.GetPosition(1));

        }
        

    }

    private void OnLinePosAdded()
    {
        

    }

}
