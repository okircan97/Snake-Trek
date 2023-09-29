using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

// This class is to create a typing effect for the texts for the game over panel.
public class TextHandler : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    // Fields for the typing.
    public float typingSpeed = 50.0f;
    public string draoclineStr;
    public string enemyStr;
    public string asteroidStr;
    public string scoreStr;
    bool playTextRunning;

    // References.
    [SerializeField] TMP_Text draoclineText;
    [SerializeField] TMP_Text enemyText;
    [SerializeField] TMP_Text asteroidText;
    [SerializeField] TMP_Text scoreText;

    #endregion

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region  METHODS

    // This method is to call the TypeText.
    public void CallTypeText()
    {
        draoclineStr = PlayerPrefs.GetFloat("draoclineCollected").ToString() + " draocline collected.";
        enemyStr = PlayerPrefs.GetFloat("enemiesDestroyed").ToString() + " enemy destroyed.";
        asteroidStr = PlayerPrefs.GetFloat("asteroidsDestroyed").ToString() + " asteroids destroyed.";
        scoreStr = "Score: " + PlayerPrefs.GetFloat("score").ToString();

        Debug.Log("TEXTHANDLER TEXTHANDLER TEXTHANDLER TEXTHANDLER TEXTHANDLER TEXTHANDLER");
        Debug.Log(draoclineStr);
        Debug.Log(enemyStr);
        Debug.Log(asteroidStr);
        Debug.Log(scoreStr);

        StartCoroutine(PlayText(draoclineText, draoclineStr));
        StartCoroutine(PlayText(enemyText, enemyStr));
        StartCoroutine(PlayText(asteroidText, asteroidStr));
        StartCoroutine(PlayText(scoreText, scoreStr));
    }

    IEnumerator PlayText(TMP_Text textComponent, string str)
    {
        foreach (char c in str)
        {
            playTextRunning = true;
            textComponent.text += c;
            yield return new WaitForSeconds(0.05f);
            playTextRunning = false;
        }
    }

    #endregion
}
