using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    [SerializeField] GameObject[] asteroidPrefabs;
    [SerializeField] float secondsBetweenAstreoids;
    float asteroidTimer;

    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float secondsBetweenEnemies;
    float enemyTimer;

    [SerializeField] Vector2 forceRange;
    Camera mainCamera;

    public float increaseInterval = 1.0f; // Time interval for increasing fields
    public int fieldToIncrease = 0;       // Index of the field to increase
    public float decreaseAmount = 0.01f;   // Amount to increase the field by
    private float timer = 0.0f;
    float secondsBetweenIncreaseDiff = 1f;

    #endregion

    // ////////////////////////////////////////
    // //////////// START & UPDATE ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS

    void Start()
    {
        mainCamera = Camera.main;
        gameObject.SetActive(false);
    }

    void Update()
    {
        CallSpawnAsteroids();
        CallSpawnEnemies();
        CallIncreaseDifficulty();
    }

    #endregion

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region METHODS

    #region ASTEROID STUFF

    // This method is to call the SpawnAsteroids method using a counter.
    void CallSpawnAsteroids()
    {
        asteroidTimer -= Time.deltaTime;
        if (asteroidTimer <= 0)
        {
            SpawnAsteroids();
            asteroidTimer = secondsBetweenAstreoids;
        }
    }

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

    #endregion

    #region ENEMY STUFF

    // This method is to call the SpawnEnemies method using a counter.
    void CallSpawnEnemies()
    {
        enemyTimer -= Time.deltaTime;
        if (enemyTimer <= 0)
        {
            SpawnEnemies();
            enemyTimer = secondsBetweenEnemies;
        }
    }

    void SpawnEnemies()
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

        // Spawn a random enemy.
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = -10;
        GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemyInstance = Instantiate(enemy,
                                                worldSpawnPoint,
                                                Quaternion.Euler(0f, 90f, Random.Range(70, 110)));

        // Add velocity to the enemyship and rotate it towards the velocity.
        Rigidbody rb = enemyInstance.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
        enemyInstance.transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
    }
    #endregion

    #endregion

    void CallIncreaseDifficulty()
    {
        // Update the timer
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            IncreaseDifficulty();
            timer = secondsBetweenIncreaseDiff;
        }
    }

    private void IncreaseDifficulty()
    {
        if (secondsBetweenAstreoids >= 1f)
        {
            secondsBetweenAstreoids -= decreaseAmount;
        }

        if (secondsBetweenEnemies >= 1f)
        {
            secondsBetweenEnemies -= decreaseAmount;
        }
    }
}
