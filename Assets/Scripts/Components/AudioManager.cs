using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [Header("BGM")]
    [SerializeField]
    private AudioClip[] bgmClips; // 0 : 기본 스테이지, 1 :보스 스테이지
    [SerializeField][Range(0, 1)]
    private float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("SFX")]
    [SerializeField]
    private AudioClip[] sfxClips;
    [SerializeField][Range(0, 1)]
    private float sfxVolume;
    [SerializeField]
    private int channels;
    private int channelIndex;
    private AudioSource[] sfxPlayers;

    public enum Sfx { Jump, Shoot, Slide, Hit, Respawn, Die, Button, GameOver }

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

        Init();
    }

    void Init()
    {
        // BGM 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = true;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[0];

        // SFX 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    /*
    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "button":
                GetComponent<AudioSource>().clip = buttonClip;
                break;
            case "gameOver":
                GetComponent<AudioSource>().clip = gameOverClip;
                break;
        }

        GetComponent<AudioSource>().Play();
    }
    */
}