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

    MainMenuHandler menuHandler;
    [SerializeField] TMP_Text priceText;
    [SerializeField] Transform snakeMenu;
    int snakeIndex = 0;
    int snakeKey = 0;

    // Price, shield, and speed
    float[,] snakeProperties = {{0,30,5},{3500,30,5},{3500,30,5.2f},{4500,40,5.4f},{4500,40,5.4f},
                             {5000,40,5.4f},{5000,40,5.4f},{6000,50,5.5f},{6500,50,5.5f},{6500,50,5.6f},
                             {6500,50,5.7f},{10000,60,5.8f},{10000,60,6f},{10000,70,6.2f},{10000,70,6.3f},
                             {125000,80,6.5f},{14000,90,7f},{14000,80,7f},{20000,90,8f},{20000,90,8f}};

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
