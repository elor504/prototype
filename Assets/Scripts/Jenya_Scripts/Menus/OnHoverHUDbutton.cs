using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class OnHoverHUDbutton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Color newColor;
    TextMeshPro m_TextMeshPro;



    void Start()
    {
        newColor = new Color(3f, 45f, 67f);
        m_TextMeshPro = gameObject.GetComponent<TextMeshPro>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        m_TextMeshPro.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_TextMeshPro.color = Color.white;
    }
}
