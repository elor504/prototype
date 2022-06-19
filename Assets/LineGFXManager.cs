using System;
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
       if(RopePhysic.getInstance.isRopeActive)
        OnLinePosAdded();

        if (ghostMove.startMovement)
        {
            LineFollowGhost();
        }

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

                
                for (int i = 0; i <= lineGFXList.Count - 1; i++)
                {
                    if (mainLine.GetPosition(i) != null && mainLine.positionCount > i + 1)
                    {
                        lineGFXList[i].SetPosition(0, mainLine.GetPosition(i));
                        lineGFXList[i].SetPosition(1, mainLine.GetPosition(i + 1));

                    }

                }

                //Debug.LogError("Delete to point");

                DeleteToPointNumber();

            }
            else if (mainLine.positionCount == 0 && ghostMove.ropeGFXBool == false)
            {
                //Debug.LogError("Reset");
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

    public void RemoveLastLinesBeforeLastRune(List<Vector2> _ghostPath)
	{
        List<Vector2> ghostPath = _ghostPath;
        lineDots = 0;
        while (ghostPath.Count != lineGFXList.Count)
		{
            Destroy(lineGFXList[lineGFXList.Count - 1].gameObject);
            lineGFXList.RemoveAt(lineGFXList.Count - 1);
        }
    }

    public void LineFollowGhost()
    {
        if (lineGFXList.Count > 0)
        {
            lineGFXList[0].SetPosition(0, ghostMove.rb.position);

            if (Vector2.Distance(lineGFXList[0].GetPosition(0), lineGFXList[0].GetPosition(1)) < 0.5f)
            {
                Destroy(lineGFXList[0].gameObject);
                lineGFXList.RemoveAt(0);
            }

        }

    }

    public void DeleteToPointNumber()
    {
        if (lineGFXList.Count >= mainLine.positionCount)
        {
            for (int i = lineGFXList.Count+1 - mainLine.positionCount; i > 0; i--)
            {
                //Debug.LogError("FORFORFOR");
                Destroy(lineGFXList[lineGFXList.Count -1].gameObject);
                lineGFXList.RemoveAt(lineGFXList.Count - 1);
                activeGFXLine = lineGFXList[lineGFXList.Count - 1];

            }

            for (int i = 0; i <= lineGFXList.Count - 1; i++)
            {
                lineGFXList[i].SetPosition(0, mainLine.GetPosition(i));
                lineGFXList[i].SetPosition(1, mainLine.GetPosition(i + 1));

            }

        }

    }

}
