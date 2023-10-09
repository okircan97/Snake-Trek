using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region  FIELDS

    // Segment fields.
    List<Transform> segments;
    public Transform segmentPrefab;
    bool hasGrownThisFrame = false, hasCrashThisFrame = false;
    [SerializeField] GameObject tracker;
    List<Vector3> positionHistory = new List<Vector3>();

    // Ship fields.
    int shield = 20;
    int smallShield = 10;
    Rigidbody rb;

    // VFX
    [SerializeField] GameObject explosion;

    // Laser fields.
    [SerializeField] float secondsBetweenAstreoids;
    float fireTimer;
    [SerializeField] GameObject[] laserPrefabs;
    [SerializeField] GameObject gun;

    // References
    Camera mainCamera;
    Snake snake;
    AudioManager audioManager;
    public AudioClip explodeClip;
    public AudioClip laserClip;
    public AudioClip asteroidExpClip;
    [SerializeField] AudioSource audioHandler;

    #endregion

    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region  MONOBEHAVIORS

    void Start()
    {
        mainCamera = Camera.main;
        snake = FindObjectOfType<Snake>();

        //        Snake stuff.
        // Add the head as the first segment.
        segments = new List<Transform>();
        segments.Add(tracker.transform);

        rb = GetComponent<Rigidbody>();

        if (tag == "EnemyS")
        {
            shield = smallShield;
        }

        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        // If the enemy is tagged "EnemyS", turn it around its Z-axis.
        if (tag == "EnemyS")
        {
            transform.Rotate(Vector3.forward * 60 * Time.deltaTime);
        }

        // Make the enemies fire.
        CallFire();

        // Destroy the enemies when they're out of the viewport.
        DestroyAsteroidsOutOfViewport();
    }

    void FixedUpdate()
    {
        MoveSegments();
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
        if (other.transform.parent != null)
        {
            if (!hasCrashThisFrame && other.gameObject.transform.parent.GetComponent<Asteroid>())
            {
                other.gameObject.transform.parent.GetComponent<Asteroid>().Explode();
                shield -= 10;
                hasCrashThisFrame = true;
                if (shield <= 0)
                {
                    PlayExplosion();
                    AudioManager.Instance.PlayClip(explodeClip);
                    // PlayAudioClip(explodeClip);
                    DestroySegments();
                    Destroy(gameObject, 0.5f);
                }
            }
        }

        // If the other is "segment" or "Snake" game over.
        if (other.transform.gameObject.tag == "Segment" || other.gameObject.GetComponent<Snake>() || other.gameObject.GetComponent<Enemy>())
        {
            PlayAudioClip(explodeClip);
            PlayExplosion();
            DestroySegments();
            Destroy(gameObject);
        }

        // If the other is a player segment, increase the snake.enemiesDestroyed.
        if (other.transform.parent != null)
            if (other.transform.parent.tag == "PlayerSegments")
            {
                snake.enemiesDestroyed++;
                snake.score += 20;
            }


    }

    #endregion

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

    // This method is to destroy the asteroids when they're
    // outside of the viewport.
    void DestroyAsteroidsOutOfViewport()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1.1f || viewportPosition.x < -0.1f || viewportPosition.y > 1.1f || viewportPosition.y < -0.1f)
        {
            DestroySegments();
            Destroy(gameObject);
        }
    }


    // This method is to grow the snake as it's eating.
    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segments.Add(segment);
    }

    // This method is to apply force to the player, according to the movement dir.
    void MoveSegments()
    {
        // Store the positions in a list, so that the segments can follow them.
        positionHistory.Insert(0, tracker.transform.position);

        // Move the segments.
        int index = 0;
        foreach (var segment in segments)
        {
            Vector3 point = positionHistory[Mathf.Min(index * 5, positionHistory.Count - 1)];
            segment.transform.position = point;
            index++;
        }
    }

    // This method is to play the explosion effect.
    void PlayExplosion()
    {
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        for (int i = 0; i < explosion.transform.childCount; i++)
        {
            explosion.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
        Destroy(gameObject);
    }

    // This method is to destroy the segments.
    void DestroySegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].transform.gameObject);
        }
    }

    // This method is to make the enemies fire laser.
    void Fire()
    {
        // Get a random laser.
        GameObject laser = laserPrefabs[Random.Range(0, laserPrefabs.Length)];

        // Instantiate the laser.
        GameObject laserInstance = Instantiate(laser,
                                                gun.transform.position,
                                                Quaternion.identity);
        Rigidbody laserRB = laserInstance.GetComponent<Rigidbody>();

        // Give velocity to the laser
        laserRB.velocity = rb.velocity.normalized * 8f;

        AudioManager.Instance.PlayClip(laserClip);
        // PlayAudioClip(laserClip);
    }

    void CallFire()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            Fire();
            fireTimer = secondsBetweenAstreoids;
        }
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
