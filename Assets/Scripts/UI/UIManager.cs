using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Control")]
    [SerializeField]
    private Button jumpButton;
    [SerializeField]
    private Text jumpCountText;
    [SerializeField]
    private Button slideButton;
    [SerializeField]
    private Text slideCoolText;

    [Header("Status")]
    [SerializeField]
    private Image[] hpIcons;

    [Header("System")]
    [SerializeField]
    private GameObject dangerUI;
    [SerializeField]
    private Animator progressCha;
    [SerializeField]
    private int progressPosXMin;
    [SerializeField]
    private int progressPosXMax;
    [SerializeField]
    private float progressPosY;

    [Header("UI")]
    [SerializeField]
    private GameObject gameoverUI;
    [SerializeField]
    private GameObject pauseUI;
    public Button restartButton;
    [SerializeField]
    private GameObject controllers;

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

    public void ProgressModify(int score)
    {
        float progress = (float)score / (float)GameManager.instance.maxScore;
        int xPos = (int)Mathf.Lerp(progressPosXMin, progressPosXMax, progress);
        progressCha.transform.localPosition = new Vector2(xPos, progressPosY);
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

    public void ProgressCha(bool live)
    {
        progressCha.SetBool("onLive", live);
    }

    public void ShowDangerPanel()
    {
        dangerUI.SetActive(true);
    }

    public void ShowController(bool onShow)
    {
        controllers.SetActive(onShow);
    }
}