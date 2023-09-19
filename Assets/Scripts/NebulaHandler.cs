using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebulaHandler : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    MenuHandler menuHandler;

    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        HandleNebula();
    }

    // This method is to change the nebula sprite according
    //  to the nebula key inside the player prefs.
    public void HandleNebula()
    {
        int nebulaKey = PlayerPrefs.GetInt("Nebula", 0);
        Debug.Log("key: " + nebulaKey);
        Sprite[] nebulas = Resources.LoadAll<Sprite>("Nebulas");
        spriteRenderer.sprite = nebulas[nebulaKey];
    }
}
