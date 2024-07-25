using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Implement your click behavior here
        Debug.Log("Text Clicked!");
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

