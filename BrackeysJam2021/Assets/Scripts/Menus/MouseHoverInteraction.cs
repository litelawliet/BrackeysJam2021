using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHoverInteraction : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AkSoundEngine.PostEvent("MenuItemMouseOver", gameObject);
    }
}
