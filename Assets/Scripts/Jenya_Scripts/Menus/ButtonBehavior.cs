using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform bRectTransform;
    Vector3 onhoverScale;
    Vector3 originScale;
    [SerializeField] float onHoverScale;

    void Start()
    {
        originScale = transform.localScale; 
        bRectTransform = GetComponent<RectTransform>();
        onhoverScale = new Vector3(onHoverScale, onHoverScale, onHoverScale);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        bRectTransform.localScale = onhoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bRectTransform.localScale = originScale;
    }
}
