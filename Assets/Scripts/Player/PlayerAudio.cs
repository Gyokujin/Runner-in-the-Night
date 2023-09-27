using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField]
    private AudioClip jumpClip;
    [SerializeField]
    private AudioClip slideClip;
    [SerializeField]
    private AudioClip damageClip;
    [SerializeField]
    private AudioClip respawnClip;
    [SerializeField]
    private AudioClip dieClip;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "jump":
                audio.clip = jumpClip;
                break;
            case "slide":
                audio.clip = slideClip;
                break;
            case "damage":
                audio.clip = damageClip;
                break;
            case "respawn":
                audio.clip = respawnClip;
                break;
            case "die":
                audio.clip = dieClip;
                break;
        }

        audio.Play();
    }
}