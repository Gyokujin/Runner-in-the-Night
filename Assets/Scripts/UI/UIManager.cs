using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Player")]
    [SerializeField]
    private Button jumpButton;
    [SerializeField]
    private Text jumpCountText;
    [SerializeField]
    private Button slideButton;
    [SerializeField]
    private Text slideCoolText;

    [Header("UI")]
    [SerializeField]
    private Image[] hpIcons;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject gameoverUI;
    [SerializeField]
    private GameObject pauseUI;
    public Button restartButton;

    [Header("FX")]
    [SerializeField]
    private GameObject playerSpawnFX;

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

    public void JumpCount(int count)
    {
        jumpCountText.text = count.ToString();

        if (count == 2)
        {
            jumpButton.interactable = true;
        }
        else if (count == 0)
        {
            jumpButton.interactable = false;
        }
    }

    public void ButtonCooldown(string buttonName, float coolTime)
    {
        switch (buttonName)
        {
            case "slide":
                StartCoroutine("SlideCool", coolTime);
                break;
        }
    }

    IEnumerator SlideCool(float cool)
    {
        slideButton.interactable = false;
        slideCoolText.gameObject.SetActive(true);
        float time = cool;

        while (time > 0)
        {
            time -= Time.deltaTime;
            slideCoolText.text = ((int)time).ToString();
            yield return null;
        }

        slideButton.interactable = true;
        slideCoolText.gameObject.SetActive(false);
    }

    public void ScoreModify(int score)
    {
        string printScore = string.Format("{0:D4}", score);
        scoreText.text = printScore;
    }

    public void DamageUI(int index)
    {
        GameObject hpIcon = hpIcons[index].gameObject;
        hpIcon.GetComponent<Animator>().enabled = true;
    }

    public void RespawnFX(Vector2 playerPos)
    {
        Vector2 offset = playerSpawnFX.GetComponent<Offset>().offsetPos;
        playerSpawnFX.transform.position = offset + playerPos;
        playerSpawnFX.SetActive(false);
        playerSpawnFX.SetActive(true);
    }

    public void ShowGameOverPanel(bool onShow)
    {
        gameoverUI.SetActive(onShow);
    }

    public void ShowPausePanel(bool onShow)
    {
        pauseUI.SetActive(onShow);
    }
}