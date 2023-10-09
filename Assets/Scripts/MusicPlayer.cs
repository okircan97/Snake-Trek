using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////

    void Awake()
    {
        SetUpSingleton();
    }

    //////////////////////////////////
    //////////// METHODS /////////////
    //////////////////////////////////

    // This method is to set up the singleton design.
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
