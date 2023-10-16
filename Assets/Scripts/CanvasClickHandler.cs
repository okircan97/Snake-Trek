using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickHandler : MonoBehaviour, IPointerDownHandler
{
    public SimpleJoystick joystick;  // Drag your FixedJoystick here from the Inspector

    public void OnPointerDown(PointerEventData eventData)
    {
        joystick.HandlePointerDown(eventData);
    }
}