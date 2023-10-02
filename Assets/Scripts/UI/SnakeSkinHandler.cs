using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

// This class is to handle the ship skin.
public class SnakeSkinHandler : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Price, shield, and speed
    float[,] snakeProperties = {{0,30,5},{1000,30,5},{1000,30,5.1f},{1500,40,5.2f},{1500,40,5.3f},
                             {2000,40,5.4f},{2000,40,5.4f},{2500,50,5.5f},{2500,50,5.5f},{3000,50,5.6f},
                             {3000,50,5.6f},{3500,60,5.6f},{3500,60,5.7f},{4000,70,5.8f},{4000,70,5.9f},
                             {4500,80,6f},{4500,90,6.1f},{4500,80,6.2f},{5000,90,6.3f},{5000,90,6.3f}};

    void Awake()
    {
        HandleSnakeSkin();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This method is to change the snake model according
    // to the snake key inside the player prefs. It'll also
    // change the isTrigger property of its colliders to true.
    public void HandleSnakeSkin()
    {
        int snakeKey = PlayerPrefs.GetInt("Snake", 0);
        Debug.Log("key: " + snakeKey);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == snakeKey)
            {
                // Activate the ship model.
                transform.GetChild(i).gameObject.SetActive(true);

                // Update the Snake's properties accordingly.
                if (transform.GetComponent<Snake>())
                {
                    transform.GetComponent<Snake>().shield = snakeProperties[i, 1];
                    transform.GetComponent<Snake>().maxVelocity = snakeProperties[i, 2];
                }

                // Each model has a child with multiple childs containing colliders.
                int colliderNum = transform.GetChild(i).gameObject.transform.GetChild(0).childCount;

                // Traverse amongst the childs and set their isTrigger property to true.
                for (int j = 0; j < colliderNum; j++)
                {
                    transform.GetChild(i).gameObject.transform.GetChild(0).transform.GetChild(j).GetComponent<BoxCollider>().isTrigger = true;
                }

            }
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}