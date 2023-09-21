using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class SnakeSkinHandler : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    MainMenuHandler menuHandler;

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
        menuHandler = FindObjectOfType<MainMenuHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This method is to handle the shop menu texts, if
    // the current scene is scene 0.
    public void HandleShop(int i)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject.FindWithTag("PriceText").GetComponent<TMP_Text>().text = "Price: " + snakeProperties[i, 0].ToString();
            GameObject.FindWithTag("ShieldText").GetComponent<TMP_Text>().text = "Shield: " + snakeProperties[i, 1].ToString();
            GameObject.FindWithTag("SpeedText").GetComponent<TMP_Text>().text = "Speed: " + snakeProperties[i, 2].ToString();
        }
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

                // Handle the shop texts (for main menu only).
                HandleShop(i);

                // Update the Snake's properties accordingly.
                if (transform.GetComponent<Snake>())
                {
                    transform.GetComponent<Snake>().shield = snakeProperties[i, 1];
                    transform.GetComponent<Snake>().maxVelocity = snakeProperties[i, 2];
                }

                // Change the snake segment's material with the model's material.
                // if (transform.GetComponent<Snake>())
                // {
                //     transform.GetComponent<Snake>().segmentPrefab.GetComponent<Renderer>().material =
                //     transform.GetChild(i).gameObject.GetComponent<Renderer>().material;
                // }

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
