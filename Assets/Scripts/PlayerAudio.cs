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
    private AudioClip damageClip;

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
            case "damage":
                audio.clip = damageClip;
                break;
        }

        audio.Play();
    }

    void Update()
    {
        
    }
}