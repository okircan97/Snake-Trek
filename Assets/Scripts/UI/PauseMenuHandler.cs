using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS
    SceneHandler sceneHandler;
    [SerializeField] GameObject pauseMenu;
    Snake snake;

    #endregion


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS
    void Start()
    {
        Time.timeScale = 1;
        sceneHandler = FindObjectOfType<SceneHandler>();
        snake = FindObjectOfType<Snake>();
    }
    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

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
            snake.isStarted = true;
        }
        else
        {
            Time.timeScale = 0;
            snake.isStarted = false;
        }
    }
    #endregion
}
