using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class LeaderboardHandler : MonoBehaviour
{
    [SerializeField] TMP_Text first;
    // [SerializeField] TMP_Text second;
    // [SerializeField] TMP_Text third;
    // [SerializeField] TMP_Text fourth;
    // [SerializeField] TMP_Text fifth;

    // Start is called before the first frame update
    void Start()
    {
        float currentScore = PlayerPrefs.GetFloat("currentScore", 0);
        float score = PlayerPrefs.GetFloat("score", 0);

        if (score > currentScore)
        {
            PlayerPrefs.SetFloat("currentScore", score);
        }

        first.text = PlayerPrefs.GetFloat("currentScore", 0).ToString();

        // float[] scores = GetScores();
        // first.text = scores[0].ToString();
        // second.text = scores[1].ToString();
        // third.text = scores[2].ToString();
        // fourth.text = scores[3].ToString();
        // fifth.text = scores[4].ToString();   

    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method is to get the player prefs for the score.
    float[] GetScores()
    {
        // The score from the previous game.
        float score = PlayerPrefs.GetFloat("score", 0);

        // A list representing the 5 best score.
        float[] scores = { 0, 0, 0, 0, 0 };

        for (int i = 1; i < 6; i++)
        {
            // If current score is bigger than any, put it to the 5th score
            // player pref.
            if (PlayerPrefs.GetFloat("score" + i.ToString(), 0) < score)
            {
                Debug.Log("element" + i + "is " + scores[i]);
                Debug.Log("score is :" + score);
                Debug.Log("element" + i + "is the smaller one");
                PlayerPrefs.SetFloat("score5", score);
                break;
            }
        }

        // Create a new list with the updated player prefs.
        scores[0] = PlayerPrefs.GetFloat("score1", 0);
        scores[1] = PlayerPrefs.GetFloat("score2", 0);
        scores[2] = PlayerPrefs.GetFloat("score3", 0);
        scores[3] = PlayerPrefs.GetFloat("score4", 0);
        scores[4] = PlayerPrefs.GetFloat("score5", 0);

        // Sort the scores.
        Array.Sort(scores);
        Array.Reverse(scores);

        // Put them back into the player prefs.
        for (int i = 0; i < scores.Length; i++)
        {
            Debug.Log("scores[" + i + "]: " + scores[i]);
            PlayerPrefs.SetFloat("score" + i.ToString(), scores[i]);
        }

        return scores;
    }
}
