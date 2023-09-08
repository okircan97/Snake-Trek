using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider gridArea;

    // Start is called before the first frame update
    void Start()
    {
        RandomizePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method is to get a random coordinate between the given bounds.
    void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        float randX = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float randY = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        transform.position = new Vector3(randX, randY, 0);
    }

    // Change the food's position after the snake collides with it.
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Snake>())
            RandomizePosition();
    }
}

