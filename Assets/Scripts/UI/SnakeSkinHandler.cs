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
    float[,] snakeProperties = {{0,30,5},{3500,30,5},{3500,30,5.2f},{4500,40,5.4f},{4500,40,5.4f},
                             {5000,40,5.4f},{5000,40,5.4f},{6000,50,5.5f},{6500,50,5.5f},{6500,50,5.6f},
                             {6500,50,5.7f},{10000,60,5.8f},{10000,60,6f},{10000,70,6.2f},{10000,70,6.3f},
                             {125000,80,6.5f},{14000,90,7f},{14000,80,7f},{20000,90,8f},{20000,90,8f}};

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

                // Handle the shield and speed texts in the menu.
                HandleShowTexts(i);
            }
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void HandleShowTexts(int i)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject.FindWithTag("ShieldShowText").GetComponent<TMP_Text>().text = "Shield: " + snakeProperties[i, 1].ToString();
            GameObject.FindWithTag("SpeedShowText").GetComponent<TMP_Text>().text = "Speed: " + snakeProperties[i, 2].ToString();
        }
    }
}
