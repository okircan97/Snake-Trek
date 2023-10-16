using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector2 inputVector = Vector2.zero;
    private RectTransform joystickBG;
    private RectTransform joystick;
    private Camera mainCamera;
    private Vector3 initialPosition; // store initial position of joystickBG
    private bool isDragging = false;
    private PointerEventData cachedEventData;

    private void Start()
    {
        joystickBG = GetComponent<RectTransform>();
        joystick = transform.GetChild(0).GetComponent<RectTransform>();
        mainCamera = Camera.main;
        initialPosition = joystickBG.position; // set the initial position
    }

    void Update()
    {
        if (isDragging && cachedEventData != null)
        {
            OnDrag(cachedEventData);
        }

        if (Input.touchCount == 0)
        {
            isDragging = false;
            joystickBG.anchoredPosition = initialPosition;
            joystick.anchoredPosition = Vector2.zero;
            cachedEventData = null;
        }
    }

    void LateUpdate()
    {
        if (!isDragging)
        {
            joystickBG.anchoredPosition = Vector3.Lerp(joystickBG.anchoredPosition, initialPosition, Time.deltaTime * 10f);
            joystick.anchoredPosition = Vector3.Lerp(joystick.anchoredPosition, Vector2.zero, Time.deltaTime * 10f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBG.sizeDelta.x);
            pos.y = (pos.y / joystickBG.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystick.anchoredPosition = new Vector2(inputVector.x * (joystickBG.sizeDelta.x / 3), inputVector.y * (joystickBG.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        cachedEventData = eventData;
        Vector2 touchPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        joystickBG.position = touchPosition; // Set joystick background to touch position
        Debug.Log("you've touched the screen: " + touchPosition);

        OnDrag(eventData); // We still call OnDrag to update the joystick handle's position.
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp called!");
        isDragging = false;
        cachedEventData = null;
        inputVector = Vector2.zero;
        joystick.anchoredPosition = Vector2.zero;
        joystickBG.position = initialPosition; // return the joystickBG to the initial position
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }

    public void HandlePointerDown(PointerEventData eventData)
    {
        OnPointerDown(eventData);
        OnDrag(eventData); // Immediately start dragging
    }
}