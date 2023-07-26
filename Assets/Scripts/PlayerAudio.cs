using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField]
    private AudioClip jumpClip;
    [SerializeField]
    private AudioClip damageClip;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void JumpSound()
    {
        audio.clip = jumpClip;
        audio.Play();
    }

    void Update()
    {
        
    }
}