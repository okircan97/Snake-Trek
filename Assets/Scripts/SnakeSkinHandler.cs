using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeSkinHandler : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    MenuHandler menuHandler;

    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        HandleSnakeSkin();
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

                // Change the snake segment's material with the model's material.
                if (transform.GetComponent<Snake>())
                {
                    transform.GetComponent<Snake>().segmentPrefab.GetComponent<Renderer>().material =
                    transform.GetChild(i).gameObject.GetComponent<Renderer>().material;
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
