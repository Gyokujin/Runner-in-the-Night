using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public enum StageType
    {
        run, battle
    }

    public StageType stageType;

    [Header("StageInfo")]
    private int score = 0;
    public bool isGameOver = false;

    [Header("UI")]
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject gameoverUI;
    [SerializeField]
    private Button restartButton;

    [Header("AnimatorType")]
    [SerializeField]
    private AnimatorController runAnimator;
    [SerializeField]
    private AnimatorController battleAnimator;

    [Header("Component")]
    [SerializeField]
    private PlayerController player;

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
    }

    void Start()
    {
        stageType = StageType.run;
        ChangeAnimator();
    }

    void ChangeAnimator()
    {
        Animator playerAnimator = player.GetComponent<Animator>();

        switch (stageType)
        {
            case StageType.run:
                playerAnimator.runtimeAnimatorController  = runAnimator;
                break;
            case StageType.battle:
                playerAnimator.runtimeAnimatorController = battleAnimator;
                break;
        }
    }

    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        Invoke("GameOverProcess", 2f);
    }

    void GameOverProcess()
    {
        gameoverUI.SetActive(true);
        SoundManager.instance.PlaySound("gameOver");
    }

    public void GameRestart()
    {
        if (isGameOver)
        {
            restartButton.interactable = false;
            SoundManager.instance.PlaySound("button");
            Invoke("GameRestartProcess", 1f);
        }
    }

    void GameRestartProcess()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}