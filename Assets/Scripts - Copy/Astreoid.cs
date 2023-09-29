using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astreoid : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    Camera mainCamera;


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

        // Destroy the asteroids when they're out of the viewport.
        DestroyAsteroidsOutOfViewport();
    }

    // Destroy the androids when they are off the screen.
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    void Crash()
    {
        gameObject.SetActive(false);
    }

    // This method is to destroy the asteroids when they're
    // outside of the viewport.
    void DestroyAsteroidsOutOfViewport()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // if (viewportPosition.x > 1.1f || viewportPosition.x < -0.1f || viewportPosition.y > 1.1f || viewportPosition.y < -0.1f)
        // {
        //     Debug.Log("asteroid yok ki");
        //     Destroy(gameObject);
        // }

        if (viewportPosition.x > 1.1f)
        {
            Debug.Log("asteroid yok ki");
            Destroy(gameObject);
        }
        if (viewportPosition.x < -0.1f)
        {
            Debug.Log("asteroid yok ki");
            Destroy(gameObject);
        }

        // Wrap the player vertically.
        if (viewportPosition.y > 1.1f)
        {
            Debug.Log("asteroid yok ki");
            Destroy(gameObject);
        }
        if (viewportPosition.y < -0.1f)
        {
            Debug.Log("asteroid yok ki");
            Destroy(gameObject);
        }

    }
}
