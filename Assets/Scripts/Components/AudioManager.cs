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
    private AudioSource bgmAudio;

    [Header("System")]
    [SerializeField]
    private AudioClip[] systemClips;
    [SerializeField][Range(0, 1)]
    private float systemVolume;
    private AudioSource systemAudio;

    [Header("Player")]
    [SerializeField]
    private AudioClip[] playerClips;
    [SerializeField][Range(0, 1)]
    private float playerVolume;
    [SerializeField]
    private int playerCh;
    private int playerIndex;
    private AudioSource[] playerAudios;

    [Header("Enemy")]
    [SerializeField]
    private AudioClip[] enemyClips;
    [SerializeField][Range(0, 1)]
    private float enemyVolume;
    [SerializeField]
    private int enemyCh;
    private int enemyIndex;
    private AudioSource[] enemyAudios;

    public enum Sfx { Jump, Shoot, Slide, Hit, Respawn, Die }

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
        GameObject bgmObject = new GameObject("BgmAudio");
        bgmObject.transform.parent = transform;
        bgmAudio = bgmObject.AddComponent<AudioSource>();
        bgmAudio.loop = true;
        bgmAudio.volume = bgmVolume;
        bgmAudio.clip = bgmClips[0];
        bgmAudio.Play();

        // System 초기화
        GameObject systemObject = new GameObject("SystemAudio");
        systemObject.transform.parent = transform;
        systemAudio = systemObject.AddComponent<AudioSource>();
        systemAudio.loop = false;
        systemAudio.volume = systemVolume;

        // Player 초기화
        GameObject playerObject = new GameObject("PlayerAudio");
        playerObject.transform.parent = transform;
        playerAudios = new AudioSource[playerCh];

        for (int i = 0; i < playerCh; i++)
        {
            playerAudios[i] = playerObject.AddComponent<AudioSource>();
            playerAudios[i].playOnAwake = false;
            playerAudios[i].volume = playerVolume;
        }

        // Enemy 초기화
        GameObject enemyObject = new GameObject("EnemyAudio");
        enemyObject.transform.parent = transform;
        enemyAudios = new AudioSource[enemyCh];

        for (int j = 0; j < enemyCh; j++)
        {
            enemyAudios[j] = enemyObject.AddComponent<AudioSource>();
            enemyAudios[j].playOnAwake = false;
            enemyAudios[j].volume = enemyVolume;
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