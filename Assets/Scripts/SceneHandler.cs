using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    SceneHandler instance;

    void Start()
    {
        // Set up the singleton.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        LoadMainMenuOnEsc();
    }

    // This method is to load the main menu on "ESC" key down.
    public void LoadMainMenuOnEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 1)
            LoadMainMenu();
    }

    // This method is to load the main menu.
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
