using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageManager : MonoBehaviour
{
    public static BossStageManager instance = null;

    [HideInInspector]
    public bool isLive = false;

    //void ArriveBoss()
    //{
    //    isArrive = true;
    //    GameLive(false);
    //    player.Move(false);
    //    UIManager.instance.ShowController(false);
    //    StartCoroutine("ArriveBossProcess");
    //}

    //IEnumerator ArriveBossProcess()
    //{
    //    UIManager.instance.ShowDangerPanel();
    //    yield return StartCoroutine(EventManager.instance.DangerEvent());
    //    yield return StartCoroutine(UIManager.instance.FadeOut());
    //    platform.PlatformChange();

    //    yield return new WaitForSeconds(1f);
    //    yield return StartCoroutine(UIManager.instance.FadeIn());
    //    yield return StartCoroutine(EventManager.instance.BossEvent());
    //    yield return StartCoroutine(UIManager.instance.FadeOut());

    //    yield return new WaitForSeconds(0.5f);
    //    PoolManager.instance.SpawnBoss(PoolManager.Boss.Excel);
    //    yield return StartCoroutine(UIManager.instance.FadeIn());

    //    GameLive(true);
    //    player.Move(true);
    //    UIManager.instance.ShowController(true);
    //}

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