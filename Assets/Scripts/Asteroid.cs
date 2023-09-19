using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    // ////////////////////////////////////////
    // //////////// START & UPDATE ////////////
    // ////////////////////////////////////////

    void Update()
    {
        // Rotate the asteroids.
        transform.Rotate(Vector3.up * (Random.Range(5, 30) * Time.deltaTime));
    }

    // ////////////////////////////////////////
    // /////////////// TRIGGERS ///////////////
    // ////////////////////////////////////////

    // private void OnTriggerEnter(Collider other)
    // {
    //     Player player = other.GetComponent<Player>();
    //     Debug.Log("I'm triggered dude!");
    //     if (player == null)
    //         return;
    //     else
    //         player.Crash();
    // }

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
}
