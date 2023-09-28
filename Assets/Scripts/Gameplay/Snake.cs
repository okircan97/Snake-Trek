using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class Snake : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region  FIELDS

    // Fields for the snake
    [SerializeField] float forceMagnitude;
    public float maxVelocity;
    Vector3 movementDirection;
    public float shield;

    // Fields for the snake segments.
    List<Transform> segments;
    public Transform segmentPrefab;
    bool hasGrownThisFrame = false, hasCrashThisFrame = false;
    [SerializeField] GameObject tracker;  // The segments will follow this obj.
    GameObject playerSegments;            // This obj. will hold all the snake segments belong to the player.

    // Fields for segment movement.
    List<Vector3> positionHistory = new List<Vector3>();
    public int gap = 5;
    bool isGrowBefore = false;

    // References.
    Camera mainCamera;
    Rigidbody rb;
    SceneHandler sceneHandler;
    [SerializeField] TMP_Text shieldText;
    [SerializeField] GameObject explosion;
    PauseMenuHandler pauseMenuHandler;
    Animator animator;
    AsteroidSpawner asteroidSpawner;
    [SerializeField] GameObject gameOverMenu;
    TextHandler textHandler;
    AudioSource audioSource;
    public AudioClip explodeClip;
    public AudioClip asteroidExpClip;
    [SerializeField] AudioSource audioHandler;
    CameraShake cameraShake;

    // Fields for calculating the score.
    public int asteroidsDestroyed;
    public int enemiesDestroyed;
    public int draoclineCollected;
    public float score;

    #endregion

    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region  MONOBEHAVIORS

    void Start()
    {
        // Initilazing the fields.
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sceneHandler = FindObjectOfType<SceneHandler>();
        pauseMenuHandler = FindObjectOfType<PauseMenuHandler>();
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
        playerSegments = GameObject.FindWithTag("PlayerSegments");
        textHandler = FindObjectOfType<TextHandler>();
        cameraShake = FindObjectOfType<CameraShake>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = explodeClip;

        // Add the head as the first segment.
        segments = new List<Transform>();
        segments.Add(tracker.transform);

        // Handle the text fields.
        shieldText.text = "Shield: " + shield.ToString();
    }

    void Update()
    {
        // Handle the movement inputs.
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();

        // Wait for the animator to play the ship animation on beginning. When
        // it's done, enable asteroidSpawner so that it could spawn asteroids.
        if (animator.enabled == true)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.enabled = false;
                asteroidSpawner.gameObject.SetActive(true);
            }
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
            draoclineCollected++;
            Grow();
            hasGrownThisFrame = true;
        }

        // If the "other" is asteroid take damage and destroy if no shield remains.
        if (!hasCrashThisFrame && other.gameObject.GetComponent<Asteroid>())
        {
            PlayAudioClip(asteroidExpClip);
            cameraShake.ShakeCamera();
            // other.gameObject.GetComponent<Asteroid>().audioSource.Play();
            other.gameObject.GetComponent<Asteroid>().Explode();
            shield -= 10;
            shieldText.text = "Shield: " + shield.ToString();
            hasCrashThisFrame = true;
            if (shield <= 0)
            {
                // audioSource.Play();
                GameOver();
            }
        }

        // If the "other" is laser take damage and destroy if no shield remains.
        if (!hasCrashThisFrame && other.gameObject.GetComponent<Laser>())
        {
            cameraShake.ShakeCamera();
            shield -= 10;
            shieldText.text = "Shield: " + shield.ToString();
            hasCrashThisFrame = true;
            if (shield <= 0)
            {
                // audioSource.Play();
                GameOver();
            }
        }

        // If the other is "segment" or an "enemy", game over.
        if (other.transform.gameObject.tag == "Segment" || other.gameObject.GetComponent<Enemy>() || other.gameObject.GetComponent<Laser>())
        {
            audioSource.Play();
            GameOver();
        }
    }

    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

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

        // Move the head.
        if (movementDirection != Vector3.zero)
        {
            rb.AddForce(movementDirection * forceMagnitude, ForceMode.Impulse);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }

        // Store the positions in a list, so that the segments can follow them.
        positionHistory.Insert(0, tracker.transform.position);

        // Move the segments.
        int index = 0;
        foreach (var segment in segments)
        {
            Vector3 point = positionHistory[Mathf.Min(index * gap, positionHistory.Count - 1)];
            segment.transform.position = point;
            index++;
        }

    }

    // This method is to grow the snake as it's eating.
    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        if (!isGrowBefore)
        {
            segment.tag = "Untagged";
            isGrowBefore = true;
        }

        segment.SetParent(playerSegments.transform);
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

    // This method is to rotate the snake to face to the velocity. When velocity is zero,
    // it'll rotate to "Vector3.up".
    void RotateToFaceVelocity()
    {
        Quaternion targetRotation;
        if (rb.velocity == Vector3.zero)
            targetRotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
        else
            targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    // This method is to show the pause menu on game over.
    public void LoadGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        textHandler.CallTypeText();
    }

    // When the player is crashed, deactivate the snake, destroy the segments and 
    // play the explosion effect.
    public void GameOver()
    {
        // Store the draocline found, enemy destroyed and asteroid destroyed values
        // inside the player prefs.
        SetPlayerPrefs();

        // Calculate the score and the credits and set them at player prefs.
        // SetPlayerPrefs(CalculateScoreAndCredits());
        CalculateScoreAndCredits();

        // Destroy the segments.
        DestroySegments();

        PlayAudioClip(explodeClip);

        // Deactivate the snake.
        gameObject.SetActive(false);

        // Play the explosion effect (Explosion obj. has three particle effects as children)
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        for (int i = 0; i < explosion.transform.childCount; i++)
        {
            explosion.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }

        // Load the game over menu.
        Invoke("LoadGameOverMenu", 2f);
    }

    // This method is to destroy the segments.
    void DestroySegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].transform.gameObject);
        }
    }

    // This method is to calculate the final score and the credits collected
    // and return them in a list.
    void CalculateScoreAndCredits()
    {
        score = asteroidsDestroyed * 10 + enemiesDestroyed * 20 + draoclineCollected * 10;
        PlayerPrefs.SetFloat("score", score);

        float creditsCollected = draoclineCollected * 50;
        float credits = PlayerPrefs.GetFloat("credits", 0) + creditsCollected;
        PlayerPrefs.SetFloat("credits", credits);
    }

    // This method is to store the necessary variables inside the player prefs.
    void SetPlayerPrefs()
    {
        PlayerPrefs.SetFloat("asteroidsDestroyed", asteroidsDestroyed);
        PlayerPrefs.SetFloat("enemiesDestroyed", enemiesDestroyed);
        PlayerPrefs.SetFloat("draoclineCollected", draoclineCollected);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (clip != null)
        {
            // Assign the specified audio clip to the AudioSource
            audioHandler.clip = clip;

            // Play the audio clip
            audioHandler.Play();
        }
    }

    #endregion
}