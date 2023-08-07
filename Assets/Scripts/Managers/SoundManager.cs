using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    private AudioSource audio;

    [Header("BGM")]
    [SerializeField]
    private AudioClip bgmClip;

    [Header("SFX")]
    [SerializeField]
    private AudioClip buttonClip;
    [SerializeField]
    private AudioClip gameOverClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        audio = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "button":
                audio.clip = buttonClip;
                break;
            case "gameOver":
                audio.clip = gameOverClip;
                break;
        }

        audio.Play();
    }
}
