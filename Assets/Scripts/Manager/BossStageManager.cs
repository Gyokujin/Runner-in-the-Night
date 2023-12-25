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
        excel.GetComponent<BoxCollider2D>().enabled = true;
        excel.transform.position = bossPos;
        excel.transform.rotation = Quaternion.identity;
        excel.transform.localScale = bossScale;
    }

    //public void BossDefeat()
    //{
    //    GameLive(false);
    //    player.gameObject.layer = 7; // 이 부분 수정과 Player의 무적 실행 함수를 수정
    //    player.Move(false);
    //    UIManager.instance.ShowController(false);
    //    StartCoroutine("BossDefeatProcess");
    //}

    //IEnumerator BossDefeatProcess()
    //{
    //    yield return StartCoroutine(UIManager.instance.FadeOut());
    //    yield return new WaitForSeconds(1f);
    //    yield return StartCoroutine(UIManager.instance.FadeIn());
    //    yield return null;

    //    yield return StartCoroutine(EventManager.instance.BossDefeat());
    //    yield return StartCoroutine(UIManager.instance.FadeOut());
    //    yield return StartCoroutine(UIManager.instance.GameFinishMessage());

    //    SceneManager.LoadScene(0);
    //}
}