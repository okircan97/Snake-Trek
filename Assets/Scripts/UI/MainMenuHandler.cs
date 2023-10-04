using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    // References.
    Camera mainCamera;
    Transform camPos;
    NebulaHandler nebulaHandler;
    SnakeSkinHandler snakeSkinHandler;
    [SerializeField] Image lockImage;
    NebulaShowcase nebulaShowcase;
    SnakeShowcase snakeShowcase;

    // Fields for the nebula and snake menus.
    [SerializeField] GameObject snakePanel, nebulaPanel, snakeBuyMenu, nebulaBuyMenu;
    [SerializeField] GameObject snakeButton, nebulaButton;
    public Sprite[] nebulas;
    public Sprite[] snakes;
    string nebulaKey = "Nebula";
    string snakeKey = "Snake";

    // Animation
    public GameObject transitionPanel;

    // Audio
    AudioSource audioSource;

    #endregion


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS
    void Awake()
    {
        // Game scene should be running when it's loaded.
        Time.timeScale = 1;

        // The references.
        nebulaHandler = FindObjectOfType<NebulaHandler>();
        snakeSkinHandler = FindObjectOfType<SnakeSkinHandler>();
        nebulaShowcase = FindObjectOfType<NebulaShowcase>();
        snakeShowcase = FindObjectOfType<SnakeShowcase>();
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        // Nebula and snake sprites.
        nebulas = Resources.LoadAll<Sprite>("Nebulas");
        snakes = Resources.LoadAll<Sprite>("Snakes");

        // Handle the nebula, snake and credit objects in the hierarhy.
        HandleNebulaPanel();
        HandleSnakePanel();
        HandleCreditTexts();
    }

    void Update()
    {
        // Handle camera movement.
        HandleCamera();
    }
    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region METHODS

    #region CAMERA STUFF
    // This method is to direct the camera to the given transform pos.
    public void MoveCamera(Transform targetTransform)
    {
        camPos = targetTransform;
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
    #endregion

    #region NEBULA STUFF

    // This method is to handle the initialization process of the
    // nebula buttons inside the nebula panel.
    public void HandleNebulaPanel()
    {
        for (int i = 0; i < nebulas.Length; i++)
        {
            // Instantiate the nebula buttons
            GameObject nButton = Instantiate(nebulaButton) as GameObject;
            nButton.GetComponent<Image>().sprite = nebulas[i];
            nButton.name = i.ToString();
            nButton.transform.SetParent(nebulaPanel.transform, false);

            // First nebula is unlocked by default.
            if (i == 0)
            {
                nButton.transform.GetChild(0).transform.gameObject.SetActive(false); // Deactivate the lock image.
                nButton.GetComponent<Button>().onClick.AddListener(() => SetNebulaPlayerPref(int.Parse(nButton.name)));
            }

            // Check player pref to see if the nebula is bought.
            else
            {
                string nebulaKey = "Nebula" + i.ToString();
                // PlayerPrefs.SetInt(nebulaKey, 0);    // Set the nebula keys to 0 for test purposes.

                // If nebula is not bought add onclick listener to show the nebula menu.
                if (PlayerPrefs.GetInt(nebulaKey, 0) == 0)
                {
                    nButton.transform.GetChild(0).transform.gameObject.SetActive(true);
                    nButton.GetComponent<Button>().onClick.AddListener(() => ShowNebulaBuyMenu(int.Parse(nButton.name)));
                }

                // If nebula is bought add onclick listener to change the nebula.
                else
                {
                    nButton.transform.GetChild(0).transform.gameObject.SetActive(false);
                    nButton.GetComponent<Button>().onClick.AddListener(() => SetNebulaPlayerPref(int.Parse(nButton.name)));
                }
            }
        }
    }

    // This method is to show the nebula buy menu.
    public void ShowNebulaBuyMenu(int index)
    {
        PlayerPrefs.SetInt("NebulaToShow", index);
        nebulaShowcase.HandleNebulaShowcase();
        MoveCamera(nebulaBuyMenu.transform);

    }

    // This method is to change the specified player pref
    // for the nebula.
    public void SetNebulaPlayerPref(int index)
    {
        PlayerPrefs.SetInt(nebulaKey, index);
        nebulaHandler.HandleNebula();
    }

    // This method is to unlock the nebula buttons.
    public void UnlockNebulaButton(int index)
    {
        nebulaPanel.transform.GetChild(index).transform.GetChild(0).transform.gameObject.SetActive(false);
        nebulaPanel.transform.GetChild(index).GetComponent<Button>().onClick.RemoveAllListeners();
        nebulaPanel.transform.GetChild(index).GetComponent<Button>().onClick.AddListener(() => SetNebulaPlayerPref(index));
    }

    #endregion

    #region SNAKE STUFF

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
            sButton.transform.SetParent(snakePanel.transform, false);

            // First snake is unlocked by default.
            if (i == 0)
            {
                sButton.transform.GetChild(0).transform.gameObject.SetActive(false);
                sButton.GetComponent<Button>().onClick.AddListener(() => SetSnakePlayerPref(int.Parse(sButton.name)));
            }

            // Check player pref to see if the snake is bought.
            else
            {
                string snakeKey = "Snake" + i.ToString();

                // If snake is not bought
                if (PlayerPrefs.GetInt(snakeKey, 0) == 0)
                {
                    sButton.transform.GetChild(0).transform.gameObject.SetActive(true);
                    sButton.GetComponent<Button>().onClick.AddListener(() => ShowSnakeBuyMenu(int.Parse(sButton.name)));
                }

                // If snake is bought
                else
                {
                    sButton.transform.GetChild(0).transform.gameObject.SetActive(false);
                    sButton.GetComponent<Button>().onClick.AddListener(() => SetSnakePlayerPref(int.Parse(sButton.name)));
                }
            }
        }
    }

    // This method is to show the nebula buy menu.
    public void ShowSnakeBuyMenu(int index)
    {
        PlayerPrefs.SetInt("SnakeToShow", index);
        snakeShowcase.HandleSnakeShowcase();
        MoveCamera(snakeBuyMenu.transform);

    }

    // This method is to change the specified player pref
    // for the snake and move the camera to the buy menu.
    public void SetSnakePlayerPref(int index)
    {
        PlayerPrefs.SetInt(snakeKey, index);
        snakeSkinHandler.HandleSnakeSkin();
        // MoveCamera(snakeBuyMenu.transform);
    }

    // This method is to unlock the snake buttons.
    public void UnlockSnakeButton(int index)
    {
        snakePanel.transform.GetChild(index).transform.GetChild(0).transform.gameObject.SetActive(false);
        snakePanel.transform.GetChild(index).GetComponent<Button>().onClick.RemoveAllListeners();
        snakePanel.transform.GetChild(index).GetComponent<Button>().onClick.AddListener(() => SetSnakePlayerPref(index));
    }

    #endregion

    // This method is to update the credit texts.
    public void HandleCreditTexts()
    {
        GameObject[] creditTexts;
        creditTexts = GameObject.FindGameObjectsWithTag("CreditText");

        for (int i = 0; i < creditTexts.Length; i++)
        {
            creditTexts[i].GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("credits", 0).ToString();
        }
    }

    // This method is to load the game after playing the transition animation.
    IEnumerator PlayAnimAndLoadScene()
    {
        audioSource.Play();
        transitionPanel.SetActive(true);
        transitionPanel.GetComponent<Animator>().SetTrigger("sceneLoading");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    // A method to call PlayAnimAndLoadScene onclick.
    public void CallPlayAnimAndLoadScene()
    {
        StartCoroutine(PlayAnimAndLoadScene());
    }

    #endregion
}
