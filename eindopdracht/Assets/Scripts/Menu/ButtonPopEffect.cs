using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPopEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float popScale = 1.1f;
    public float popSpeed = 8f;
    private Vector3 originalScale;
    private Vector3 targetScale;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * popSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * popScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}