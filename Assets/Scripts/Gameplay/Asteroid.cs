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
    Snake snake;
    public AudioClip explodeClip;
    AudioManager audioManager;

    #endregion


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS

    void Start()
    {
        mainCamera = Camera.main;
        snake = FindObjectOfType<Snake>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        // Rotate the asteroids.
        transform.Rotate(Vector3.up * (Random.Range(5, 30) * Time.deltaTime));
        DeactivateAsteroidsOutOfViewport();
    }

    private void OnCollisionEnter(Collision other)
    {
        // explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        // explosion.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

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
                snake.score += 10;
            }
        }

        AudioManager.Instance.PlayClip(explodeClip);

        Explode();
        ResetAsteroidState();
    }
    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region METHODS

    // Modified function name for clarity
    void DeactivateAsteroidsOutOfViewport()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1.1f || viewportPosition.x < -0.1f || viewportPosition.y > 1.1f || viewportPosition.y < -0.1f)
        {
            ResetAsteroidState();
        }
    }

    // New method to reset the asteroid to its initial state
    void ResetAsteroidState()
    {
        transform.GetComponent<MeshRenderer>().enabled = true;
        if (transform.childCount > 0)
            transform.GetChild(0).gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    // This method is to play the explosion effect of the asteroids.
    public void Explode()
    {

        // Fetch an explosion object from the object pool
        GameObject pooledExplosion = ObjectPooler.Instance.SpawnFromPool("Explosion", transform.position, Quaternion.identity);
        AudioManager.Instance.PlayClip(explodeClip);

        // if (pooledExplosion)
        // {
        //     // Ensuring it's active in case it wasn't before
        //     pooledExplosion.SetActive(true);
        // }

        ResetAsteroidState();
    }
    #endregion
}
