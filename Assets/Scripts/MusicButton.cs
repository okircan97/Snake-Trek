using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    MusicPlayer musicPlayer;
    Image btnImage;

    [SerializeField] Sprite musicOnImg;
    [SerializeField] Sprite musicOffImg;

    void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        btnImage = GetComponent<Image>();
    }

    // This method is to stop/play the music.
    public void HandleMusic()
    {
        AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            btnImage.sprite = musicOffImg;
        }

        else
        {
            audioSource.UnPause();
            btnImage.sprite = musicOnImg;
        }
    }
}
