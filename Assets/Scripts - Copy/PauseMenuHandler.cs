using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    SceneHandler sceneHandler;
    [SerializeField] GameObject pauseMenu;


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////

    void Start()
    {
        Time.timeScale = 1;
        sceneHandler = FindObjectOfType<SceneHandler>();
    }


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////

    // This method is to load the main menu.
    public void LoadMainMenu()
    {
        sceneHandler.LoadMainMenu();
    }

    // This method is to restart the level.
    public void Restartlevel()
    {
        sceneHandler.LoadGame();
    }

    // This method is to open the stop/close panel and pause/resume the game.
    public void ResumePauseGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (!pauseMenu.activeSelf)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
