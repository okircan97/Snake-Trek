using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class AsteroidSpawner : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    [Header("Spawning Settings")]
    [SerializeField] GameObject[] asteroidPrefabs;
    [SerializeField] float secondsBetweenAstreoids;
    float asteroidTimer;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float secondsBetweenEnemies;
    float enemyTimer;
    [SerializeField] Vector2 forceRange;

    [Header("Difficulty Settings")]
    public float increaseInterval = 1.0f;  // Time interval for increasing fields
    public int fieldToIncrease = 0;        // Index of the field to increase
    public float decreaseAmount = 0.01f;   // Amount to increase the field by
    private float timer = 0.0f;
    float secondsBetweenIncreaseDiff = 1f;

    // References
    Camera mainCamera;

    // Warning stuff
    public GameObject warningPrefab; // Prefab for the warning icon (like an exclamation point)
    public float warningDuration = 0.5f; // How long the warning lasts

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

    IEnumerator ShowWarningThenSpawn(Vector2 spawnPoint, System.Action<Vector2> spawnCallback)
    {
        Vector2 warningPos = new Vector2(spawnPoint.x, spawnPoint.y);

        // Decide which border the spawn point is closest to
        if (warningPos.x < 0.5f && warningPos.x <= warningPos.y && warningPos.x <= (1 - warningPos.y))
        {
            warningPos.x = 0.05f; // left
        }
        else if (warningPos.x >= 0.5f && warningPos.x >= warningPos.y && warningPos.x >= (1 - warningPos.y))
        {
            warningPos.x = 0.95f; // right
        }
        else if (warningPos.y < 0.5f)
        {
            warningPos.y = 0.05f; // bottom
        }
        else
        {
            warningPos.y = 0.95f; // top
        }

        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(warningPos);
        worldSpawnPoint.z = -10;

        GameObject warningInstance = ObjectPooler.Instance.SpawnFromPool(warningPrefab.tag, worldSpawnPoint, Quaternion.identity);
        yield return new WaitForSeconds(warningDuration);

        // Disable the warning message
        warningInstance.SetActive(false);

        // Call the provided spawn callback with the same spawn point
        spawnCallback(spawnPoint);
    }

    Vector2 GetSpawnPoint()
    {
        int side = Random.Range(0, 4);
        Vector2 spawnPoint = Vector2.zero;
        switch (side)
        {
            case 0:
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                break;
            case 1:
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                break;
            case 2:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                break;
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                break;
        }
        return spawnPoint;
    }

    Vector2 GetDirectionFromSpawnPoint(Vector2 spawnPoint)
    {
        Vector2 direction = Vector2.zero;
        if (spawnPoint.x == 0)
        {
            direction = new Vector2(1f, Random.Range(-1f, 1f));
        }
        else if (spawnPoint.x == 1)
        {
            direction = new Vector2(-1f, Random.Range(-1f, 1f));
        }
        else if (spawnPoint.y == 0)
        {
            direction = new Vector2(Random.Range(-1f, 1f), 1f);
        }
        else if (spawnPoint.y == 1)
        {
            direction = new Vector2(Random.Range(-1f, 1f), -1f);
        }
        return direction;
    }


    #region ASTEROID STUFF

    // This method is to call the SpawnAsteroids method using a counter.
    void CallSpawnAsteroids()
    {
        asteroidTimer -= Time.deltaTime;
        if (asteroidTimer <= 0)
        {
            Vector2 spawnPoint = GetSpawnPoint();
            StartCoroutine(ShowWarningThenSpawn(spawnPoint, SpawnAsteroids));
            asteroidTimer = secondsBetweenAstreoids;
        }
    }

    // This method is to spawn asteroids.
    void SpawnAsteroids(Vector2 spawnPoint)
    {
        Vector2 direction = GetDirectionFromSpawnPoint(spawnPoint);
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = -10;

        GameObject asteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        GameObject asteroidInstance = ObjectPooler.Instance.SpawnFromPool(asteroid.tag, worldSpawnPoint, Quaternion.Euler(0f, 0f, Random.Range(10, 150)));
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
            Vector2 spawnPoint = GetSpawnPoint();
            StartCoroutine(ShowWarningThenSpawn(spawnPoint, SpawnEnemies));
            enemyTimer = secondsBetweenEnemies;
        }
    }

    void SpawnEnemies(Vector2 spawnPoint)
    {
        Vector2 direction = GetDirectionFromSpawnPoint(spawnPoint);
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = -10;

        GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemyInstance = ObjectPooler.Instance.SpawnFromPool(enemy.tag, worldSpawnPoint, Quaternion.Euler(0f, 90f, Random.Range(70, 110)));
        Rigidbody rb = enemyInstance.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
        enemyInstance.transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.forward);
    }

    #endregion

    #region DIFFICULTY STUFF
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

    #endregion

    #endregion

}
