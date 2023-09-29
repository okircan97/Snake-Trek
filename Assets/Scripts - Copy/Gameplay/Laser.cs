using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    Camera mainCamera;


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region  MONOBEHAVIORS

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Make the lasers face the velocity.
        transform.LookAt(transform.position + GetComponent<Rigidbody>().velocity);

        // Destroy the enemies when they're out of the viewport.
        DestroyLasersOutOfViewport();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Food>())
        {
            other.transform.GetComponent<Food>().RandomizePosition();
        }
        Destroy(gameObject);
    }

    #endregion

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

    // This method is to destroy the asteroids when they're
    // outside of the viewport.
    void DestroyLasersOutOfViewport()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1.1f || viewportPosition.x < -0.1f || viewportPosition.y > 1.1f || viewportPosition.y < -0.1f)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
