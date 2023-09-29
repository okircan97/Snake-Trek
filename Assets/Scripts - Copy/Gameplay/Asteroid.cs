using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    Camera mainCamera;
    [SerializeField] GameObject explosion;
    Snake snake;
    public AudioSource audioSource;
    public AudioClip explodeClip;

    #endregion


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS

    void Start()
    {
        mainCamera = Camera.main;
        snake = FindObjectOfType<Snake>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.clip = explodeClip;
    }

    void Update()
    {
        // Rotate the asteroids.
        transform.Rotate(Vector3.up * (Random.Range(5, 30) * Time.deltaTime));
        DestroyAsteroidsOutOfViewport();
    }

    private void OnCollisionEnter(Collision other)
    {
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        explosion.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        if (other.transform.GetComponent<Food>())
        {
            other.transform.GetComponent<Food>().RandomizePosition();
        }

        // If the other is a player segment, increase the snake.asteroidsDestroyed.
        if (other.transform.parent != null)
        {
            if (other.transform.parent.tag == "PlayerSegments")
            {
                snake.asteroidsDestroyed++;
            }
        }

        audioSource.Play();
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).transform.gameObject.SetActive(false);
        Destroy(gameObject, 1);
    }
    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region METHODS

    // This method is to destroy the asteroids when they're
    // outside of the viewport.
    void DestroyAsteroidsOutOfViewport()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1.1f || viewportPosition.x < -0.1f || viewportPosition.y > 1.1f || viewportPosition.y < -0.1f)
        {
            Destroy(gameObject);
        }
    }

    // This method is to play the explosion effect of the asteroids.
    public void Explode()
    {
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        explosion.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        // explosion.transform.SetParent(gameObject.transform);
        gameObject.GetComponent<MeshRenderer>().gameObject.SetActive(false);
    }
    #endregion
}
