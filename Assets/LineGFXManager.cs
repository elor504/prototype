﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGFXManager : MonoBehaviour
{
    private static LineGFXManager _instance;
    public static LineGFXManager LineGFXManage => _instance;

    [SerializeField] LineRenderer mainLine;
    [SerializeField] LineRenderer prefabGFXLine;

    [SerializeField] private int lineDots = 0;
    private LineRenderer activeGFXLine;
    [SerializeField] List<LineRenderer> lineGFXList = new List<LineRenderer>();

    private GhostMovement ghostMove => GameManager.getInstance.ghost;

    // Start is called before the first frame update
    void Start()
    {
        lineDots = 0;
       
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
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

                if (lineGFXList.Count >= lineDots)
                {
                    for (int i = lineGFXList.Count - lineDots; i > 0; i--)
                    {
                        Destroy(lineGFXList[lineGFXList.Count-1].gameObject);
                        lineGFXList.RemoveAt(lineGFXList.Count-1);

                    }

                }

            }
            else if (mainLine.positionCount == 0 && ghostMove.ropeGFXBool == false)
            {
                ResetLineGFX();

            }

        }

        lineDots = mainLine.positionCount;

    }

    public void ResetLineGFX()
    {
        foreach (LineRenderer item in lineGFXList)
        {
            Destroy(item.gameObject);
        }

        lineGFXList.Clear();
    }

    public void RemoveLastLineRenderer()
    {
        //Debug.LogError("REMOVED LAST LINE");
        lineDots = 0;
        Destroy(lineGFXList[lineGFXList.Count - 1].gameObject);
        lineGFXList.RemoveAt(lineGFXList.Count - 1);
    }
}
