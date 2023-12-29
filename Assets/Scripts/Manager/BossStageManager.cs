using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class BossStageManager : MonoBehaviour
{
    public static BossStageManager instance = null;

    [SerializeField]
    private B_Excel excel;
    [SerializeField]
    private Vector2 bossPos;
    [SerializeField]
    private Vector3 bossScale;

    [SerializeField]
    private GameObject excelTurbo;
    [SerializeField]
    private Vector2 turboPos;

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

    public void BossStageStart()
    {
        // System
        GameManager.instance.isLive = true;
        UIManager.instance.ShowController(true);
        AudioManager.instance.BgmPlay(AudioManager.StageBGM.BossStage);
        EventManager.instance.EndTimeLine();

        // Player
        GameManager.instance.player.Move(true);

        // Excel
        excel.gameObject.SetActive(true);
        excel.enabled = true;
        excel.GetComponent<SpriteRenderer>().flipX = false;
        excel.GetComponent<BoxCollider2D>().enabled = true;
        excel.transform.position = bossPos;
        excel.transform.rotation = Quaternion.identity;
        excel.transform.localScale = bossScale;

        // Excel - Turbo
        excelTurbo.gameObject.SetActive(true);
        excelTurbo.GetComponent<SpriteRenderer>().flipX = false;
        excelTurbo.transform.localPosition = turboPos;
        excelTurbo.transform.rotation = Quaternion.identity;
        excelTurbo.transform.localScale = Vector3.one;
    }

    public void BossDefeat()
    {
        GameManager.instance.gameFinish = true;
        GameManager.instance.GameLive(false);
        GameManager.instance.player.Move(false);
        GameManager.instance.player.gameObject.layer = 7;
        UIManager.instance.ShowController(false);
        AudioManager.instance.MuteEnemySFX(AudioManager.EnemySfx.ExcelBoost); // FlameSpear 패턴의 경우 사운드가 오래 가기 때문에 끊는다.
        excel.enabled = false;
        excel.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine("BossDefeatProcess");
    }

    IEnumerator BossDefeatProcess()
    {
        if (PlayerPrefs.GetInt("GameProgress") != 2) // 클리어 데이터를 저장한다.
        {
            PlayerPrefs.SetInt("GameProgress", 2);
        }

        // Fade Out
        yield return StartCoroutine(UIManager.instance.FadeOut());
        AudioManager.instance.MuteBgm();
        yield return new WaitForSeconds(2f);

        // Event Start
        EventManager.instance.PlayTimeLine(EventManager.Timeline.BossDefeat);
        yield return null;

        // Fade In
        StartCoroutine(UIManager.instance.FadeIn());
    }

    public void GameFinish()
    {
        GameManager.instance.player.gameObject.SetActive(false);
        excel.gameObject.SetActive(false);
        EventManager.instance.PlayTimeLine(EventManager.Timeline.GameFinish);
    }
}