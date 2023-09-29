using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NebulaShowcase : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    SpriteRenderer spriteRenderer;
    MainMenuHandler menuHandler;
    [SerializeField] TMP_Text priceText;
    int nebulaIndex = 0;
    [SerializeField] Transform nebulaMenu;
    int nebulaKey = 0;
    float nebulaPrice = 500;

    #endregion

    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS

    void Start()
    {
        menuHandler = FindObjectOfType<MainMenuHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        HandleNebulaShowcase();
    }

    #endregion

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

    // Show the nebula on shop. 
    public int HandleNebulaShowcase()
    {
        nebulaKey = PlayerPrefs.GetInt("NebulaToShow", 0);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == nebulaKey)
            {
                // Activate the nebula model.
                transform.GetChild(i).gameObject.SetActive(true);

                // Handle the price text.
                priceText.text = "Price: 500";

                nebulaIndex = i;
            }
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
        return nebulaIndex;
    }

    // This method is to buy nebula.
    public void BuyNebula()
    {
        // The player has enough credits to buy nebula.
        if (PlayerPrefs.GetFloat("credits") >= 500)
        {
            PlayerPrefs.SetInt("Nebula" + nebulaIndex.ToString(), 1);
            Debug.Log("Nebula index: " + "Nebula" + nebulaIndex.ToString());
            PlayerPrefs.SetFloat("credits", PlayerPrefs.GetFloat("credits") - 500);
            menuHandler.UnlockNebulaButton(nebulaIndex);
            menuHandler.MoveCamera(nebulaMenu);
            menuHandler.HandleCreditTexts();
        }
    }

    #endregion
}
