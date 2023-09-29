using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    [SerializeField] GameObject[] asteroidPrefabs;
    [SerializeField] float secondsBetweenAstreoids;
    [SerializeField] Vector2 forceRange;
    float timer;
    Camera mainCamera;


    // ////////////////////////////////////////
    // //////////// START & UPDATE ////////////
    // ////////////////////////////////////////

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnAsteroids();
            timer = secondsBetweenAstreoids;
        }
    }


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////

    // This method is to spawn asteroids.
    void SpawnAsteroids()
    {
        // The asteroids will randomly spawn from the any of the 4 
        // sides of the screen.
        int side = Random.Range(0, 4);

        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        // Get a random spawn point for the asteroid.
        switch (side)
        {
            // Left
            case 0:
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            // Right
            case 1:
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            // Bottom
            case 2:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            // Top
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }

        // Spawn a random asteroid.
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = -10;
        GameObject asteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        // TO DO
        // Check if the object is a ship. If so, change its rotation
        // to head where it's heading.
        GameObject asteroidInstance = Instantiate(asteroid,
                                                worldSpawnPoint,
                                                Quaternion.Euler(0f, 0f, Random.Range(10, 150)));
        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
    }
}
