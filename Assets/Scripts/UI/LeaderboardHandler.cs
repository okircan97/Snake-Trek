using System;
using TMPro;
using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    [SerializeField] TMP_Text firstPlaceText;
    [SerializeField] TMP_Text secondPlaceText;
    [SerializeField] TMP_Text thirdPlaceText;
    [SerializeField] TMP_Text fourthPlaceText;
    [SerializeField] TMP_Text fifthPlaceText;

    void Start()
    {
        UpdateLeaderboard();
    }

    // This method is to update the leaderboard using the 
    void UpdateLeaderboard()
    {
        float currentScore = PlayerPrefs.GetFloat("score", 0);
        float[] scores = GetScores(currentScore);

        firstPlaceText.text = scores[0].ToString();
        secondPlaceText.text = scores[1].ToString();
        thirdPlaceText.text = scores[2].ToString();
        fourthPlaceText.text = scores[3].ToString();
        fifthPlaceText.text = scores[4].ToString();

        PlayerPrefs.SetFloat("score", 0);
    }

    // This method is to get the scores from the player prefs in
    // descending order.
    float[] GetScores(float newScore)
    {
        float[] scores = new float[5];

        // Load existing scores
        for (int i = 0; i < 5; i++)
        {
            // scores[i] = PlayerPrefs.GetFloat("score" + (i + 1), float.MaxValue);
            scores[i] = PlayerPrefs.GetFloat("score" + (i + 1), 0);
        }

        // Check if the new score belongs in the leaderboard
        for (int i = 0; i < 5; i++)
        {
            if (newScore > scores[i])
            {
                // Insert the new score into the leaderboard
                Array.Copy(scores, i, scores, i + 1, 4 - i);
                scores[i] = newScore;
                break;
            }
        }

        // Save updated scores to PlayerPrefs
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat("score" + (i + 1), scores[i]);
        }

        return scores;
    }
}