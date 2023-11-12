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

    [Header("Respawn")]
    [SerializeField]
    private float spawnOffX;
    [SerializeField]
    private float spawnOffY;

    [Header("Component")]
    [SerializeField]
    private PlayerController player;
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
        if (!isLive)
            return;

        if (!scoreGetting)
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