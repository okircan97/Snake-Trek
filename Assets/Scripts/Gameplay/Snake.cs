using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class Snake : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region  FIELDS

    // Fields for the snake.
    [SerializeField] float forceMagnitude;
    public float maxVelocity;
    Vector3 movementDirection;
    public float shield;
    [SerializeField] TMP_Text shieldText;
    public TMP_Text scoreText;
    public bool isStarted;     // A bool to check if the game is started.

    // Fields for the snake segments.
    List<Transform> segments;
    public Transform segmentPrefab;
    bool hasGrownThisFrame = false, hasCrashThisFrame = false;
    [SerializeField] GameObject tracker;  // The segments will follow this obj.
    GameObject playerSegments;            // This obj. will hold all the snake segments belong to the player.

    // Fields for segment movement.
    List<Vector3> positionHistory = new List<Vector3>();    // A list to hold the transform pos. history.
    int gap = 5;                                     // For 
    bool isGrowBefore = false;                              // A bool to get the first segment.

    // Fields for calculating the score.
    public int asteroidsDestroyed;
    public int enemiesDestroyed;
    public int draoclineCollected;
    public float score = 0;

    // References.
    Camera mainCamera;
    Rigidbody rb;
    SceneHandler sceneHandler;
    PauseMenuHandler pauseMenuHandler;
    public Animator animator;
    TypeText typeText;
    AsteroidSpawner asteroidSpawner;
    CameraShake cameraShake;

    // Audio stuff.
    public AudioClip explodeClip;
    public AudioClip asteroidExpClip;
    public AudioClip foodClip;
    // AudioSource audioSource;
    AudioManager audioManager;

    // Animation
    [SerializeField] GameObject explosion;
    bool foodAnimPlaying = false;

    // UI
    [SerializeField] GameObject gameOverMenu;

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
        typeText = FindObjectOfType<TypeText>();
        cameraShake = FindObjectOfType<CameraShake>();
        // audioSource = GameObject.FindWithTag("AudioHandler").transform.GetComponent<AudioSource>();
        audioManager = FindObjectOfType<AudioManager>();

        // Add the head as the first segment.
        segments = new List<Transform>();
        segments.Add(tracker.transform);

        // Handle the text fields.
        shieldText.text = "Shield: " + shield.ToString();
        scoreText.text = "Score: " + score.ToString();

        animator.enabled = false;
    }

    void Update()
    {
        // Handle the movement inputs.
        if (isStarted)
        {
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                ProcessInput();
            }
            RotateToFaceVelocity();
            KeepPlayerOnScreen();
        }

        // Wait for the animator to play the ship animation on beginning. When
        // it's done, enable asteroidSpawner and turn the isStarted flag to true.
        if (animator.enabled == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.enabled = false;
                asteroidSpawner.gameObject.SetActive(true);
                isStarted = true;
            }
        }

        scoreText.text = "Score: " + score.ToString();

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
        Food food = other.gameObject.GetComponent<Food>();
        Asteroid asteroid = other.gameObject.GetComponent<Asteroid>();
        Laser laser = other.gameObject.GetComponent<Laser>();
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        // If the "other" is food, grow.
        if (!hasGrownThisFrame && food && !foodAnimPlaying)
        {
            Animator foodAnimator = food.gameObject.GetComponent<Animator>();
            StartCoroutine(PlayFoodAnim(foodAnimator, food));
        }

        // If the "other" is laser take damage and destroy if no shield remains.
        else if (!hasCrashThisFrame && laser)
        {
            Debug.Log("laser");
            cameraShake.ShakeCamera();
            shield -= 10;
            laser.gameObject.SetActive(false);
            shieldText.text = "Shield: " + shield.ToString();
            hasCrashThisFrame = true;
            if (shield <= 0)
            {
                GameOver();
            }
        }

        // If the other is "segment" or an "enemy", game over.
        else if (other.transform.gameObject.tag == "Segment" || enemy)
        {
            cameraShake.ShakeCamera();
            GameOver();
        }

        // If the "other" is asteroid take damage and destroy if no shield remains.
        else if (other.transform.parent != null)
        {
            if (!hasCrashThisFrame && other.gameObject.transform.parent.GetComponent<Asteroid>())
            {
                asteroid = other.gameObject.transform.parent.GetComponent<Asteroid>();

                AudioManager.Instance.PlayClip(asteroidExpClip);
                // PlayAudioClip(asteroidExpClip);
                cameraShake.ShakeCamera();
                asteroid.Explode();
                shield -= 10;
                shieldText.text = "Shield: " + shield.ToString();
                hasCrashThisFrame = true;
                if (shield <= 0)
                {
                    GameOver();
                }
            }
        }
    }

    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

    #region  MOVEMENT METHODS

    // This method is to process the input.
    void ProcessInput()
    {
        // // If the screen is touched:
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
        if (segments.Count() != 0)
        {
            foreach (var segment in segments)
            {
                Vector3 point = positionHistory[Mathf.Min(index * gap, positionHistory.Count - 1)];
                segment.transform.position = point;
                index++;
            }
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

    # endregion

    #region  SEGMENT AND FOOD METHODS
    // This method is to grow the snake as it's eating.
    public void Grow()
    {
        if (segments.Count != 0)
        {
            Transform segment = Instantiate(segmentPrefab, segments[segments.Count - 1].position, Quaternion.identity);
            if (!isGrowBefore)
            {
                segment.tag = "Untagged";
                isGrowBefore = true;
            }

            segment.SetParent(playerSegments.transform);
            segments.Add(segment);
        }

    }

    // Play the foor animation.
    IEnumerator PlayFoodAnim(Animator foodAnimator, Food food)
    {

        AudioManager.Instance.PlayClip(foodClip);
        food.transform.position = new Vector3(food.transform.position.x, food.transform.position.y, -5);
        foodAnimPlaying = true;
        foodAnimator.SetTrigger("Trigger");
        yield return new WaitForSeconds(1f);

        food.RandomizePosition();
        draoclineCollected++;
        score += 10;

        Grow();
        hasGrownThisFrame = true;
        foodAnimator.ResetTrigger("Trigger");

        foodAnimPlaying = false;
    }

    #endregion

    #region  GAMEOVER METHODS

    // When the player is crashed, deactivate the snake, destroy the segments and 
    // play the explosion effect.
    public void GameOver()
    {
        int snakeKey = PlayerPrefs.GetInt("Snake", 0);

        SetPlayerPrefs();
        CalculateScoreAndCredits();
        DestroySegments();

        AudioManager.Instance.PlayClip(explodeClip);
        gameObject.transform.GetChild(snakeKey).transform.gameObject.SetActive(false);

        // // Play the explosion effect (Explosion obj. has three particle effects as children)
        // explosion = Instantiate(explosion, transform.position, Quaternion.identity);

        // Using object pooling to get the explosion effect
        GameObject explosionEffect = ObjectPooler.Instance.SpawnFromPool("ShipExplosion", transform.position, Quaternion.identity);
        StartCoroutine(LoadGameOverMenu());
    }

    // This method is to store the necessary variables inside the player prefs.
    void SetPlayerPrefs()
    {
        PlayerPrefs.SetFloat("asteroidsDestroyed", asteroidsDestroyed);
        PlayerPrefs.SetFloat("enemiesDestroyed", enemiesDestroyed);
        PlayerPrefs.SetFloat("draoclineCollected", draoclineCollected);
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

    // This method is to destroy the segments.
    void DestroySegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].transform.gameObject);
        }

        segments.Clear();
    }

    // This method is to show the game over menu on game over.
    IEnumerator LoadGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        // TO DO: Play menu animation
        gameOverMenu.GetComponent<Animator>().SetTrigger("gameOver");
        yield return new WaitForSeconds(1f);

        typeText.CallTypeText();
    }

    #endregion

    #endregion

}