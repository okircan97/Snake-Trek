using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    Camera camera;
    Transform camPos;

    void Start()
    {
        camera = Camera.main;
        // camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (camPos != null)
        {
            Vector3 pos2Go = new Vector3(camPos.transform.position.x, camPos.transform.position.y, camera.transform.position.z);
            camera.transform.position = Vector3.Lerp(camera.transform.position, camPos.transform.position, 5f * Time.deltaTime);
        }

    }

    // This method is to direct the camera to the given transform.
    public void MoveCamera(Transform targetTransform)
    {
        camPos = targetTransform;
    }

}
