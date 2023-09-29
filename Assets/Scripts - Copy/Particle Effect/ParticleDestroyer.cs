using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{

    void Update()
    {
        DestroyObject();
    }

    // This method is to destroy the game obj. if there's no
    // active childs attached to it.
    void DestroyObject()
    {
        // A bool to check if there're any enabled children.
        bool isChildEnabled = false;

        // If the game object has no child, destroy it.
        if (transform.childCount == 0)
            Destroy(gameObject);

        else
        {
            // Check if there's any enabled child.
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).transform.gameObject.activeSelf)
                    isChildEnabled = true;
            }
        }

        // If there's no enabled child, destroy the game object.
        if (!isChildEnabled)
            Destroy(gameObject);

    }
}
