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
    [SerializeField] GameObject transitionMenu;
    Animator transitionAnim;

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
        transitionAnim = transitionMenu.GetComponent<Animator>();
    }

    void Update()
    {
        if (transitionAnim.enabled == true)
            if (transitionAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                transitionMenu.SetActive(false);
                snake.animator.enabled = true;
            }
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

    // T
    #endregion
}
