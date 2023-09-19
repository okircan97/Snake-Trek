using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider gridArea;

    // public GameObject[] junks;
    Vector3 rotationVector;


    void Start()
    {
        RandomizePosition();
    }

    void Update()
    {
        transform.Rotate(rotationVector * (Random.Range(5, 30) * Time.deltaTime));
    }

    // This method is to get a random coordinate between the given bounds.
    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        float randX = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float randY = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        transform.position = new Vector3(randX, randY, -10);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rotationVector = GetRandVector3();
        // SpawnJunk();
    }

    // This method is to spawn junk.
    // void SpawnJunk()
    // {
    //     // If there's a previously created junk, destroy it.
    //     if (transform.childCount != 0)
    //         Destroy(transform.GetChild(0).gameObject);

    //     // Spawn a new random junk.
    //     int rand = Random.Range(0, junks.Length);
    //     GameObject junk = Instantiate(junks[rand], transform.position, Quaternion.identity);
    //     junk.transform.localScale = new Vector3(0.05f, 0.05f, 0.1f);
    //     junk.transform.SetParent(transform);
    //     rotationVector = GetRandVector3();
    // }

    // This method is to return a random Vector3.
    Vector3 GetRandVector3()
    {
        int rand = Random.Range(0, 6);

        switch (rand)
        {
            case 0:
                return Vector3.back;
            case 1:
                return Vector3.down;
            case 2:
                return Vector3.forward;
            case 3:
                return Vector3.left;
            case 4:
                return Vector3.right;
            default:
                return Vector3.up;
        }
    }
}

