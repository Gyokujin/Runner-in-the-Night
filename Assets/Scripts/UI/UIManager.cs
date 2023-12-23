using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
    private GameObject hpIcon;
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
    [SerializeField]
    private Slider bossHPBar;
    [SerializeField]
    private Text gameFinishText;
    [SerializeField]
    private GameObject skipButton;

    [Header("Fade")]
    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private float fadeTime = 2f;

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
        hpIcon.SetActive(onShow);
    }
    
    public void ShowSkipButton(bool onShow)
    {
        skipButton.SetActive(onShow);
    }

    public void BossHPModify(bool able, int maxHp = 0, int hp = 0)
    {
        if (able) // UI 활성화
        {
            if (!bossHPBar.IsActive())
            {
                bossHPBar.gameObject.SetActive(true);
            }

            float percent = (float)hp / (float)maxHp;
            bossHPBar.value = percent;
        }
        else // UI 비활성화
        {
            bossHPBar.gameObject.SetActive(false);
        }
    }

    public IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 1);
        float time = fadeTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, time / fadeTime);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);
        float time = fadeTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, (1 - time) / fadeTime);
            yield return null;
        }
    }

    public IEnumerator GameFinishMessage()
    {
        gameFinishText.gameObject.SetActive(true);
        gameFinishText.color = new Color(1, 1, 1, 0);
        float time = 3;

        while (time > 0)
        {
            time -= Time.deltaTime;
            gameFinishText.color = new Color(1, 1, 1, (1 - time / 3));
            yield return null;
        }

        yield return new WaitForSeconds(5f);
        time = 3;

        while (time > 0)
        {
            time -= Time.deltaTime;
            gameFinishText.color = new Color(1, 1, 1, time / 3);
            yield return null;
        }
    }
}