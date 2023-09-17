using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    // Fields for the snake segments.
    List<Transform> segments;
    public Transform segmentPrefab;
    bool hasGrownThisFrame = false;
    bool isOutsideViewport = false;
    Vector3 outOfViewportPos;

    // Fields for the snake movement.
    [SerializeField] float forceMagnitude;
    [SerializeField] float maxVelocity;
    Vector3 movementDirection;

    // References.
    Camera mainCamera;
    Rigidbody rb;

    // Fields for testing purposes.
    int foodEaten = 0;

    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////

    void Start()
    {
        // Initilazing the fields.
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        segments = new List<Transform>();
        segments.Add(transform);
    }

    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateTheFaceVelocity();
    }

    void FixedUpdate()
    {
        ApplyForce();
    }

    void LateUpdate()
    {
        hasGrownThisFrame = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the snake did not grow at this frame:
        if (!hasGrownThisFrame && other.gameObject.GetComponent<Food>())
        {
            // Change the food position and grow the snake. Set the
            // flag to true.
            other.gameObject.GetComponent<Food>().RandomizePosition();
            Grow();
            hasGrownThisFrame = true;
        }
    }

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////

    // This method is to process the input.
    void ProcessInput()
    {
        // If the screen is touched:
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Get the touch pos and convert it to world pos.
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            // The player will move towards the clicked pos.
            movementDirection = worldPosition - transform.position;
            movementDirection.z = 0f;

            // When normalized, the force won't be changed overtime.
            movementDirection.Normalize();
        }
    }

    // This method is to apply force to the player, according to the movement dir.
    void ApplyForce()
    {
        // Movement of the segments.

        // If the snake is not outside the viewport, lerp the segments from
        // the snake head pos.
        if (!isOutsideViewport)
        {
            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].position = Vector3.Lerp(segments[i].position, segments[i - 1].position, 10f * Time.deltaTime);
            }

        }
        // If the snake is outside the the viewport, lerp the segments from
        // the outOfViewportPos.
        else
        {
            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].position = segments[i - 1].position;
            }
        }


        // Movement of the head.
        if (movementDirection != Vector3.zero)
        {
            rb.AddForce(movementDirection * forceMagnitude, ForceMode.Force);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    // This method is to grow the snake as it's eating.
    public void Grow()
    {
        // segments[segments.Count-1].transform.position.x
        Vector3 segmentPos = segments[segments.Count - 1].position - movementDirection;
        Transform segment = Instantiate(segmentPrefab, segmentPos, Quaternion.identity);
        // segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    // This method is to keep the player on screen.
    void KeepPlayerOnScreen()
    {
        // Get the pos and viewport pos.
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Set the isOutsideViewport flag when the snake head goes outside the viewport.
        if (viewportPosition.x > 1 || viewportPosition.x < 0 || viewportPosition.y > 1 || viewportPosition.y < 0)
        {
            isOutsideViewport = true;
            StartCoroutine(WaitFlagChanger());
        }

        // NOTE: Viewport's edges are (0,1), (1,1), (0,0) and (1,0).
        // If the player goes beyond the screen, transform it to the opposite side.
        // Wrap the player horizontally.
        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
            outOfViewportPos = transform.position;
        }
        if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
            outOfViewportPos = transform.position;
        }

        // Wrap the player vertically.
        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
            outOfViewportPos = transform.position;
        }
        if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
            outOfViewportPos = transform.position;
        }

        transform.position = newPosition;
    }

    // This method is to rotate the player to face to the velocity.
    void RotateTheFaceVelocity()
    {
        if (rb.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    // When the player is crashed, deactivate it and end the game.
    public void Crash()
    {
        gameObject.SetActive(false);
        // sceneHandler.EndGame();
    }

    // A coroutine to wait and change the isOutsideViewport.
    IEnumerator WaitFlagChanger()
    {
        yield return new WaitForSeconds(segments.Count * 0.1f);
        isOutsideViewportChanger();
    }

    // This method is to change the isOutsideViewport.
    public bool isOutsideViewportChanger()
    {
        Debug.Log("Ahanda flag'i degistirdim");
        isOutsideViewport = false;
        return isOutsideViewport;
    }
}

