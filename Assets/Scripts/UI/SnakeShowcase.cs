using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SnakeShowcase : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    SpriteRenderer spriteRenderer;
    MainMenuHandler menuHandler;
    [SerializeField] TMP_Text priceText;
    int snakeIndex = 0;
    [SerializeField] Transform snakeMenu;
    int snakeKey = 0;

    // Price, shield, and speed
    float[,] snakeProperties = {{0,30,5},{1000,30,5},{1000,30,5.1f},{1500,40,5.2f},{1500,40,5.3f},
                             {2000,40,5.4f},{2000,40,5.4f},{2500,50,5.5f},{2500,50,5.5f},{3000,50,5.6f},
                             {3000,50,5.6f},{3500,60,5.6f},{3500,60,5.7f},{4000,70,5.8f},{4000,70,5.9f},
                             {4500,80,6f},{4500,90,6.1f},{4500,80,6.2f},{5000,90,6.3f},{5000,90,6.3f}};

    #endregion


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS

    void Awake()
    {
        HandleSnakeShowcase();
    }


    // Start is called before the first frame update
    void Start()
    {
        menuHandler = FindObjectOfType<MainMenuHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

    // This method is to change the snake model according
    // to the snake key inside the player prefs. It'll also
    // change the isTrigger property of its colliders to true.
    public int HandleSnakeShowcase()
    {
        int snakeKey = PlayerPrefs.GetInt("SnakeToShow", 0);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == snakeKey)
            {
                // Activate the ship model.
                transform.GetChild(i).gameObject.SetActive(true);

                // Handle the shop texts (for main menu only).
                HandleShopTexts(i);

                snakeIndex = i;
            }
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
        return snakeIndex;
    }

    // This method is to handle the shop menu texts, if
    // the current scene is scene 0.
    public void HandleShopTexts(int i)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject.FindWithTag("PriceText").GetComponent<TMP_Text>().text = "Price: " + snakeProperties[i, 0].ToString();
            GameObject.FindWithTag("ShieldText").GetComponent<TMP_Text>().text = "Shield: " + snakeProperties[i, 1].ToString();
            GameObject.FindWithTag("SpeedText").GetComponent<TMP_Text>().text = "Speed: " + snakeProperties[i, 2].ToString();
        }
    }

    public void BuySnake()
    {
        if (PlayerPrefs.GetFloat("credits") >= snakeProperties[snakeIndex, 0])
        {
            PlayerPrefs.SetInt("Snake" + snakeIndex.ToString(), 1);
            Debug.Log("Snake index: " + "Snake" + snakeIndex.ToString());
            PlayerPrefs.SetFloat("credits", PlayerPrefs.GetFloat("credits") - snakeProperties[snakeIndex, 0]);
            menuHandler.UnlockSnakeButton(snakeIndex);
            menuHandler.MoveCamera(snakeMenu);
            menuHandler.HandleCreditTexts();
        }
    }

    #endregion
}
