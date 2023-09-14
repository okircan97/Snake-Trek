using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    // float speed = 10f;
    // List<Transform> segments;
    // public Transform segmentPrefab;

    // void Start()
    // {
    //     segments = new List<Transform>();
    //     segments.Add(transform);
    // }
    // void FixedUpdate()
    // {
    //     Move();
    // }

    // // This method is to move the snake.
    // public void Move()
    // {
    //     // Movement of the segments.
    //     for (int i = segments.Count - 1; i > 0; i--)
    //     {
    //         segments[i].position = segments[i - 1].position;
    //     }

    //     // Regular movement.
    //     int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
    //     int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
    //     Vector3 movementVector = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
    //     transform.Translate(movementVector);
    // }

    // // This method is to grow the snake as it's eating.
    // public void Grow()
    // {
    //     Transform segment = Instantiate(segmentPrefab);
    //     segment.position = segments[segments.Count - 1].position;
    //     segments.Add(segment);
    // }


    // private void OnCollisionEnter(Collision other)
    // {
    //     // Call the grow function on coluusion with a food.
    //     if (other.gameObject.GetComponent<Food>())
    //         Grow();
    //     // Restart the game on collusion with a wall or a segment.
    //     if (other.gameObject.tag == "Wall")
    //         SceneManager.LoadScene(0);
    // }

    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    [SerializeField] float forceMagnitude;
    [SerializeField] float maxVelocity;
    Camera mainCamera;
    Rigidbody rb;
    Vector3 movementDirection;


    // ////////////////////////////////////////
    // //////////// START & UPDATE ////////////
    // ////////////////////////////////////////

    void Start()
    {
        // Initilazing the fields.
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
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

            // The player will move against the clicked pos.
            movementDirection = worldPosition - transform.position;
            movementDirection.z = 0f;

            // When normalized, the force won't be changed overtime.
            movementDirection.Normalize();
        }

        else
        {
            // If not clicked, the force will be zero.
            movementDirection = Vector3.zero;
        }
    }

    // This method is to apply force to the player, according to the movement dir.
    void ApplyForce()
    {
        if (movementDirection != Vector3.zero)
        {
            rb.AddForce(movementDirection * forceMagnitude, ForceMode.Force);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    // This method is to keep the player on screen.
    void KeepPlayerOnScreen()
    {
        // Get the pos and viewport pos.
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // NOTE: Viewport's edges are (0,1), (1,1), (0,0) and (1,0).
        // If the player goes beyond the screen, transform it to the opposite side.
        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }
        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
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
}

