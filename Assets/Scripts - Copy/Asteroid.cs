using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    Camera mainCamera;
    [SerializeField] GameObject explosion;


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////

    void Start()
    {
        mainCamera = Camera.main;
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

        Destroy(gameObject);
    }

    // Destroy the androids when they are off the screen.
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////

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
        gameObject.SetActive(false);
    }
}
