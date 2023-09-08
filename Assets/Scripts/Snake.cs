using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    float speed = 10f;
    List<Transform> segments;
    public Transform segmentPrefab;

    void Start()
    {
        segments = new List<Transform>();
        segments.Add(transform);
    }
    void FixedUpdate()
    {
        Move();
    }

    // This method is to move the snake.
    public void Move()
    {
        // Movement of the segments.
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        // Regular movement.
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        Vector3 movementVector = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        transform.Translate(movementVector);
    }

    // This method is to grow the snake as it's eating.
    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }


    private void OnCollisionEnter(Collision other)
    {
        // Call the grow function on coluusion with a food.
        if (other.gameObject.GetComponent<Food>())
            Grow();
        // Restart the game on collusion with a wall or a segment.
        if (other.gameObject.tag == "Wall")
            SceneManager.LoadScene(0);
    }
}

