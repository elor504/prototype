using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGFXManager : MonoBehaviour
{
    [SerializeField] LineRenderer mainLine;
    [SerializeField] LineRenderer prefabGFXLine;

    [SerializeField] private int lineDots = 0;
    private LineRenderer activeGFXLine;
    [SerializeField] List<LineRenderer> lineGFXList = new List<LineRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        lineDots = 0;
       
    }

    // Update is called once per frame
    void Update()
    {
        OnLinePosAdded();
    }

    private void OnLinePosAdded()
    {
        
        if (mainLine.positionCount <= lineDots)
        {
            if (mainLine.positionCount != 0)
            {
                if (lineGFXList.Count +1 < mainLine.positionCount)
                {
                    Debug.Log("created line");

                    if (lineGFXList.Count != 0)
                    {
                        activeGFXLine.SetPosition(1, mainLine.GetPosition(lineDots-2));
                    }

                    activeGFXLine = GameObject.Instantiate(prefabGFXLine);

                    lineGFXList.Add(activeGFXLine);

                }

                activeGFXLine.SetPosition(0, mainLine.GetPosition(lineDots - 2));
                activeGFXLine.SetPosition(1, mainLine.GetPosition(lineDots - 1));

            }
            else if (mainLine.positionCount == 0)
            {
                foreach (LineRenderer item in lineGFXList)
                {
                    Destroy(item);
                }

                lineGFXList.Clear();

            }

        }

        lineDots = mainLine.positionCount;

    }

}
