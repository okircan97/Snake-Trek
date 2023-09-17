using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    Camera camera;
    Transform camPos;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        // Handle camera movement.
        if (camPos != null)
        {
            Vector3 pos2Go = new Vector3(camPos.transform.position.x, camPos.transform.position.y, camera.transform.position.z);
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos2Go, 5f * Time.deltaTime);
            if (camera.transform.position == pos2Go)
                camPos = null;
        }
    }

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
}
