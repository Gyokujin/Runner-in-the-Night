using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageManager : MonoBehaviour
{
    [SerializeField]
    private B_Excel excel;

    public void BossStageStart()
    {
        // GameManager
        GameManager.instance.isLive = true;

        // Player
        GameManager.instance.player.Move(true);

        // Excel
        excel.enabled = true;
        excel.GetComponent<BoxCollider2D>().enabled = true;
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