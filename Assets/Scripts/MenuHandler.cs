using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    Camera camera;
    Transform camPos;
    [SerializeField] private GameObject snakePanel, nebulaPanel;
    [SerializeField] private GameObject snakeButton, nebulaButton;
    NebulaHandler nebulaHandler;
    public Sprite[] nebulas;
    string nebulaKey = "Nebula";


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////

    void Awake()
    {
        nebulaHandler = FindObjectOfType<NebulaHandler>();
        camera = Camera.main;
        nebulas = Resources.LoadAll<Sprite>("Nebulas");
        HandleNebulaPanel();
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
            Vector3 pos2Go = new Vector3(camPos.transform.position.x, camPos.transform.position.y, camera.transform.position.z);
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos2Go, 5f * Time.deltaTime);
            if (camera.transform.position == pos2Go)
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
            Debug.Log("Listener with index value " + int.Parse(nButton.name) + " is created.");
            nButton.transform.SetParent(nebulaPanel.transform, false);
        }
    }

    // This method is to change the specified player pref
    // for the nebula.
    public void SetNebulaPlayerPref(int index)
    {
        PlayerPrefs.SetInt(nebulaKey, index);
        nebulaHandler.HandleNebula();
    }
}
