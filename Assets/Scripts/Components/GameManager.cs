using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isLive = false;
    // [SerializeField]
    // private float delayTime = 1.5f;

    public enum StageType
    {
        run, battle
    }

    public StageType stageType;

    [Header("StageInfo")]
    public float stageTime = 0;
    private int score = 0;
    private float scoreDelay = 0.5f;
    private bool scoreGetting = false;
    public bool isGameOver = false;

    [Header("UI")]
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject gameoverUI;
    [SerializeField]
    private Button restartButton;

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
        stageType = StageType.run;
        isLive = true;
    }
    
    void Update()
    {
        if (!isLive)
            return;

        stageTime += Time.deltaTime;

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
            string printScore = string.Format("{0:D4}", score);
            scoreText.text = printScore;
        }
    }

    IEnumerator ProgressScore()
    {
        scoreGetting = true;
        AddScore(1);

        yield return new WaitForSeconds(scoreDelay);
        scoreGetting = false;
    }

    public void GamePause()
    {
        isLive = false;
    }

    public void CameraPause()
    {
        camera.StopCamera();
    }

    public void GameResume()
    {
        //camera.ActCamera();
        isLive = true;
    }

    public void GameOver()
    {
        isGameOver = true;
        CameraPause();
        Invoke("GameOverProcess", 2f);
    }

    void GameOverProcess()
    {
        gameoverUI.SetActive(true);
        AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.GameOver);
    }

    public void GameRestart()
    {
        if (isGameOver)
        {
            restartButton.interactable = false;
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