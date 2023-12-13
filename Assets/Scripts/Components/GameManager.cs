using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [HideInInspector]
    public bool isLive = false;

    [Header("StageInfo")]
    public int maxScore;
    public int score = 0;
    [SerializeField]
    private float scoreDelay = 0.25f;
    private bool scoreGetting = false;
    [HideInInspector]
    public bool isGameOver = false;
    private bool isArrive = false;

    [Header("Respawn")]
    [SerializeField]
    private float spawnOffX;
    [SerializeField]
    private float spawnOffY;

    [Header("Component")]
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PlatformControl platform;
    [SerializeField]
    private targetCamera camera;

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

        Application.targetFrameRate = 60;
    }

    void Start()
    {
        GameLive(true);
    }
    
    void Update()
    {
        if (isLive)
        {
            ScoreProcess();
        }
    }

    void ScoreProcess()
    {
        if (!scoreGetting && !isArrive)
        {
            StartCoroutine("ProgressScore");
        }
    }

    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
            UIManager.instance.ProgressModify(score);
        }

        if (score >= maxScore)
        {
            ArriveBoss();
        }
    }

    IEnumerator ProgressScore()
    {
        scoreGetting = true;
        AddScore(1);

        yield return new WaitForSeconds(scoreDelay);
        scoreGetting = false;
    }

    public void GameLive(bool live)
    {
        isLive = live;
        UIManager.instance.ProgressCha(live);
    }

    void ArriveBoss()
    {
        isArrive = true;
        GameLive(false);
        player.Move(false);
        AudioManager.instance.SwitchBGM(2);
        UIManager.instance.ShowController(false);
        StartCoroutine("ArriveBossProcess");
    }

    IEnumerator ArriveBossProcess()
    {
        // UIManager.instance.ShowDangerPanel();
        // yield return StartCoroutine(EventManager.instance.DangerEvent());
        // yield return StartCoroutine(UIManager.instance.FadeOut());
        platform.PlatformChange();

        // yield return new WaitForSeconds(1f);
        // yield return StartCoroutine(UIManager.instance.FadeIn());
        // yield return StartCoroutine(EventManager.instance.BossEvent());
        // yield return StartCoroutine(UIManager.instance.FadeOut());

        // yield return new WaitForSeconds(0.5f);
        PoolManager.instance.SpawnBoss(PoolManager.Boss.Excel);
        // yield return StartCoroutine(UIManager.instance.FadeIn());
        yield return null; // 이후에 빼자

        GameLive(true);
        player.Move(true);
        AudioManager.instance.SwitchBGM(1);
        UIManager.instance.ShowController(true);
    }

    public void BossDefeat()
    {
        GameLive(false);
        player.gameObject.layer = 7; // 이 부분 수정과 Player의 무적 실행 함수를 수정
        player.Move(false);
        UIManager.instance.ShowController(false);
        StartCoroutine("BossDefeatProcess");
    }

    IEnumerator BossDefeatProcess()
    {
        yield return StartCoroutine(UIManager.instance.FadeOut());
        yield return new WaitForSeconds(1f);
        AudioManager.instance.SwitchBGM(1);
        StartCoroutine(EventManager.instance.BossDefeat());
        StartCoroutine(UIManager.instance.FadeIn());
    }

    public void CameraPause()
    {
        camera.StopCamera();
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.Click);
        UIManager.instance.ShowPausePanel(true);
    }

    public void GameResume()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.Click);
        UIManager.instance.ShowPausePanel(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        CameraPause();
        Invoke("GameOverProcess", 2f);
    }

    void GameOverProcess()
    {
        UIManager.instance.ShowGameOverPanel(true);
        UIManager.instance.restartButton.interactable = true;
        AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.GameOver);
    }

    public void GameRestart()
    {
        if (isGameOver)
        {
            UIManager.instance.restartButton.interactable = false;
            AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.Click);
            Invoke("GameRestartProcess", 1f);
        }
    }

    void GameRestartProcess()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}