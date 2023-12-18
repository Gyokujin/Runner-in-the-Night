using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [Header("BGM")]
    [SerializeField]
    private AudioClip[] bgmClips;
    [SerializeField][Range(0, 1)]
    private float bgmVolume;
    private AudioSource bgmAudio;
    public enum StageBGM { RunStage, BossStage }

    [Header("System")]
    [SerializeField]
    private AudioClip[] systemClips;
    [SerializeField][Range(0, 1)]
    private float systemVolume;
    [SerializeField]
    private int systemCh;
    private int systemIndex;
    private AudioSource[] systemAudios;
    public enum SystemSFX { Click, Hit, Detect, GameOver }

    [Header("Player")]
    [SerializeField]
    private AudioClip[] playerClips;
    [SerializeField][Range(0, 1)]
    private float playerVolume;
    [SerializeField]
    private int playerCh;
    private int playerIndex;
    private AudioSource[] playerAudios;
    public enum PlayerSFX { Jump, Shoot, Slide, Hit, Respawn, Die }

    [Header("Enemy")]
    [SerializeField]
    private AudioClip[] enemyClips;
    [SerializeField][Range(0, 1)]
    private float enemyVolume;
    [SerializeField]
    private int enemyCh;
    private int enemyIndex;
    private AudioSource[] enemyAudios;
    public enum EnemySfx 
    { 
        GuardianMove, GuardianDie, DiverMove, DiverDie, ChaserChase, ChaserDie, WandererDie, BlazerAttack, BlazerDie, 
        ExcelGeneralShot, ExcelImpactShot, ExcelBoostStart, ExcelBoost, ExcelBoostEnd, ExcelMachStrike
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
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
        bgmAudio.Play();

        // System 초기화
        GameObject systemObject = new GameObject("SystemAudio");
        systemObject.transform.parent = transform;
        systemAudios = new AudioSource[systemCh];

        for (int i = 0; i < systemCh; i++)
        {
            systemAudios[i] = systemObject.AddComponent<AudioSource>();
            systemAudios[i].playOnAwake = false;
            systemAudios[i].volume = systemVolume;
        }

        // Player 초기화
        GameObject playerObject = new GameObject("PlayerAudio");
        playerObject.transform.parent = transform;
        playerAudios = new AudioSource[playerCh];

        for (int j = 0; j < playerCh; j++)
        {
            playerAudios[j] = playerObject.AddComponent<AudioSource>();
            playerAudios[j].playOnAwake = false;
            playerAudios[j].volume = playerVolume;
        }

        // Enemy 초기화
        GameObject enemyObject = new GameObject("EnemyAudio");
        enemyObject.transform.parent = transform;
        enemyAudios = new AudioSource[enemyCh];

        for (int k = 0; k < enemyCh; k++)
        {
            enemyAudios[k] = enemyObject.AddComponent<AudioSource>();
            enemyAudios[k].playOnAwake = false;
            enemyAudios[k].volume = enemyVolume;
        }
    }

    public void BgmPlay(StageBGM bgm)
    {
        bgmAudio.clip = bgmClips[(int)bgm];
        bgmAudio.Play();
    }

    public void MuteBgm()
    {
        bgmAudio.Stop();
    }

    public void BgmVolumeControl(float volume)
    {
        bgmVolume = volume;
        bgmAudio.volume = bgmVolume;
    }

    public void PlaySystemSFX(SystemSFX sfx)
    {
        for (int i = 0; i < systemAudios.Length; i++)
        {
            int loopIndex = (i + systemIndex) % systemAudios.Length;

            if (systemAudios[loopIndex].isPlaying)
                continue;

            systemIndex = loopIndex;
            systemAudios[loopIndex].clip = systemClips[(int)sfx];
            systemAudios[loopIndex].Play();
            break;
        }
    }

    public void PlayPlayerSFX(PlayerSFX sfx)
    {
        for (int i = 0; i < playerAudios.Length; i++)
        {
            int loopIndex = (i + playerIndex) % playerAudios.Length;

            if (playerAudios[loopIndex].isPlaying)
                continue;

            playerIndex = loopIndex;
            playerAudios[loopIndex].clip = playerClips[(int)sfx];
            playerAudios[loopIndex].Play();
            break;
        }
    }

    public void PlayEnemySFX(EnemySfx sfx)
    {
        for (int i = 0; i < enemyAudios.Length; i++)
        {
            int loopIndex = (i + enemyIndex) % enemyAudios.Length;

            if (enemyAudios[loopIndex].isPlaying)
                continue;

            enemyIndex = loopIndex;
            enemyAudios[loopIndex].clip = enemyClips[(int)sfx];
            enemyAudios[loopIndex].Play();
            break;
        }
    }

    public void MuteEnemySFX(EnemySfx sfx)
    {
        for (int i = 0; i < enemyAudios.Length; i++)
        {
            if (enemyAudios[i].clip == enemyClips[(int)sfx])
            {
                enemyAudios[i].Stop();
                break;
            }
        }
    }
}