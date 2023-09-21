using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    // References.
    Camera mainCamera;
    Transform camPos;
    NebulaHandler nebulaHandler;
    SnakeSkinHandler snakeSkinHandler;

    // Fields for the nebula and snake menus.
    [SerializeField] GameObject snakePanel, nebulaPanel, buyMenu;
    [SerializeField] GameObject snakeButton, nebulaButton;
    public Sprite[] nebulas;
    public Sprite[] snakes;
    string nebulaKey = "Nebula";
    string snakeKey = "Snake";


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////

    void Awake()
    {
        Time.timeScale = 1;
        nebulaHandler = FindObjectOfType<NebulaHandler>();
        snakeSkinHandler = FindObjectOfType<SnakeSkinHandler>();
        mainCamera = Camera.main;
        nebulas = Resources.LoadAll<Sprite>("Nebulas");
        snakes = Resources.LoadAll<Sprite>("Snakes");
        HandleNebulaPanel();
        HandleSnakePanel();
    }

    void Update()
    {
        // Handle camera movement.
        HandleCamera();

    }


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////

    // This method is to direct the camera to the given transform pos.
    public void MoveCamera(Transform targetTransform)
    {
        camPos = targetTransform;
    }

    // This method is to load the scene with the given index.
    public void LoadGivenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    // This method is to handle the camera movement using the camPos flag.
    public void HandleCamera()
    {
        if (camPos != null)
        {
            Vector3 pos2Go = new Vector3(camPos.transform.position.x, camPos.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, pos2Go, 5f * Time.deltaTime);
            if (mainCamera.transform.position == pos2Go)
                camPos = null;
        }
    }

    // This method is to handle the initialization process of the
    // nebula buttons inside the nebula panel.
    void HandleNebulaPanel()
    {
        for (int i = 0; i < nebulas.Length; i++)
        {
            GameObject nButton = Instantiate(nebulaButton) as GameObject;
            nButton.GetComponent<Image>().sprite = nebulas[i];
            nButton.name = i.ToString();
            nButton.GetComponent<Button>().onClick.AddListener(() => SetNebulaPlayerPref(int.Parse(nButton.name)));
            nButton.transform.SetParent(nebulaPanel.transform, false);
        }
    }

    // This method is to handle the initialization process of the
    // snake buttons inside the snake panel.
    void HandleSnakePanel()
    {
        Debug.Log("Snakes length: " + snakes.Length);
        for (int i = 0; i < snakes.Length; i++)
        {
            GameObject sButton = Instantiate(snakeButton) as GameObject;
            sButton.GetComponent<Image>().sprite = snakes[i];
            sButton.name = i.ToString();
            sButton.GetComponent<Button>().onClick.AddListener(() => SetSnakePlayerPref(int.Parse(sButton.name)));
            sButton.transform.SetParent(snakePanel.transform, false);
        }
    }

    // This method is to change the specified player pref
    // for the nebula.
    public void SetNebulaPlayerPref(int index)
    {
        PlayerPrefs.SetInt(nebulaKey, index);
        nebulaHandler.HandleNebula();
    }

    // This method is to change the specified player pref
    // for the snake and move the camera to the buy menu.
    public void SetSnakePlayerPref(int index)
    {
        PlayerPrefs.SetInt(snakeKey, index);
        snakeSkinHandler.HandleSnakeSkin();
        MoveCamera(buyMenu.transform);
    }
}
