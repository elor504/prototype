using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverHUDbutton : MonoBehaviour
{
    Color newColor;




    void OnMouseOver()
    {
        newColor = new Color(3f, 45f, 67f);
        gameObject.GetComponentInChildren<TextMesh>().color = newColor;
    }

    void OnMouseExit()
    {
        gameObject.GetComponentInChildren<TextMesh>().color = Color.white;
    }
}
