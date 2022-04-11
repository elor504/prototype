using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform bRectTransform;
    Vector3 onhoverScale;
    Vector3 originScale;
    Quaternion onhoverRotate;
    [SerializeField] float onHoverScale;
    [SerializeField] float onHoverRotate;

    void Start()
    {
        originScale = transform.localScale; 
        bRectTransform = GetComponent<RectTransform>();
        onhoverScale = new Vector3(onHoverScale, onHoverScale, onHoverScale);
        onhoverRotate = new Quaternion(0f, 0f, onHoverRotate, onHoverRotate);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        bRectTransform.localScale = onhoverScale;
        bRectTransform.localRotation = onhoverRotate;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bRectTransform.localScale = originScale;
        bRectTransform.localRotation = Quaternion.identity;
    }
}
