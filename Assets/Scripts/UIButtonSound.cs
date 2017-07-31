using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{

    public void OnPointerEnter(PointerEventData ped)
    {
        AudioManager.instance.PlaySFX("MenuButtonOver");
    }

    public void OnPointerDown(PointerEventData ped)
    {
        AudioManager.instance.PlaySFX("MenuButtonClick");
    }
}
