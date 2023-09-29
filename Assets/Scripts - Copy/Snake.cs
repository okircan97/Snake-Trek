using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class Snake : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    // Fields for the snake segments.
    List<Transform> segments;
    public Transform segmentPrefab;
    bool hasGrownThisFrame = false, hasCrashThisFrame = false;
    Vector3 outOfViewportPos;
    [SerializeField] GameObject tracker;

    // Fields for the s.n.a.k.e ship.
    [SerializeField] float forceMagnitude;
    public float maxVelocity;
    Vector3 movementDirection;
    public float shield;

    // References.
    Camera mainCamera;
    Rigidbody rb;
    SceneHandler sceneHandler;
    [SerializeField] TMP_Text shieldText;
    [SerializeField] GameObject explosion;
    PauseMenuHandler pauseMenuHandler;


    // Fields for mineral, credits and time track.
    float score = 0;
    int credits = 0;
    int foodEaten = 0;
    int asteroidsDestroyed = 0;


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////

    void Start()
    {
        // Initilazing the fields.
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        sceneHandler = FindObjectOfType<SceneHandler>();
        pauseMenuHandler = FindObjectOfType<PauseMenuHandler>();

        // Add the head as the first segment.
        segments = new List<Transform>();
        segments.Add(transform);

        // Handle the text fields.
        shieldText.text = "Shield: " + shield.ToString();

    }

    void Update()
    {
        // Handle the movement inputs.
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    void FixedUpdate()
    {
        // Handle the movement of the head and the segments.
        ApplyForce();
    }

    void LateUpdate()
    {
        // Changing the flags on late update, so that OnTriggerEnter()
        // won't get triggered multiple times. 
        hasGrownThisFrame = false;
        hasCrashThisFrame = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the "other" is food, grow.
        if (!hasGrownThisFrame && other.gameObject.GetComponent<Food>())
        {
            other.gameObject.GetComponent<Food>().RandomizePosition();
            Grow();
            hasGrownThisFrame = true;
        }

        // If the "other" is asteroid take damage and destroy if no shield remains.
        if (!hasCrashThisFrame && other.gameObject.GetComponent<Asteroid>())
        {
            other.gameObject.GetComponent<Asteroid>().Explode();
            shield -= 10;
            shieldText.text = "Shield: " + shield.ToString();
            hasCrashThisFrame = true;
            if (shield <= 0)
                GameOver();
        }

        // If the other is "segment", game over.
        if (other.transform.gameObject.tag == "Segment")
        {
            GameOver();
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
        for (int i = segments.Count - 1; i > 0; i--)
        {
            if (i == 1)
            {
                segments[i].position = tracker.transform.position;
            }
            else
            {
                segments[i].position = segments[i - 1].position;
            }

        }

        // Movement of the head.
        if (movementDirection != Vector3.zero)
        {
            rb.AddForce(movementDirection * forceMagnitude, ForceMode.Impulse);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    // This method is to grow the snake as it's eating.
    public void Grow()
    {
        Vector3 segmentPos = segments[segments.Count - 1].position - movementDirection;
        Transform segment = Instantiate(segmentPrefab, segmentPos, Quaternion.identity);
        segments.Add(segment);
    }

    // This method is to keep the player on screen.
    void KeepPlayerOnScreen()
    {
        // Get the pos and viewport pos.
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // NOTE: Viewport's edges are (0,1), (1,1), (0,0) and (1,0).
        // If the player goes beyond the screen, transform it to the opposite side.
        // Wrap the player horizontally.
        if (viewportPosition.x > 1.1f)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        if (viewportPosition.x < -0.1f)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        // Wrap the player vertically.
        if (viewportPosition.y > 1.1f)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        if (viewportPosition.y < -0.1f)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        transform.position = newPosition;
    }

    // This method is to rotate the snake and its segments to face to the velocity.
    void RotateToFaceVelocity()
    {
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    // When the player is crashed, deactivate the snake, destroy the segments and 
    // play the explosion effect.
    public void GameOver()
    {
        gameObject.SetActive(false);

        // Explosion obj. has three particle effects as children.
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        for (int i = 0; i < explosion.transform.childCount; i++)
        {
            explosion.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }

        DestroySegments();
        Invoke("LoadPauseMenu", 3f);
    }

    // This method is to destroy the segments.
    void DestroySegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].transform.gameObject);
        }
    }

    // This method is to show the pause menu on game over.
    public void LoadPauseMenu()
    {
        pauseMenuHandler.ResumePauseGame();
    }
}

