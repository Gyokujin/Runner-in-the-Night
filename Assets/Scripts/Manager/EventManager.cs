using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;

    public enum Timeline
    {
        Countdown,
        Danger,
        BossAppear,
        BossDefeat,
        GameFinish
    }

    private PlayableDirector director;
    [SerializeField]
    private TimelineAsset[] timelines;

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

        director = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (director.playableAsset == null)
            return;

        if (director.playableAsset.name == "BossAppear" || director.playableAsset.name == "BossDefeat")
        {
            SkipButtonAct();
        }
    }

    public void PlayTimeLine(Timeline index)
    {
        director.Stop();
        director.playableAsset = timelines[(int)index];
        director.Play();
    }

    public void EndTimeLine()
    {
        UIManager.instance.ShowSkipButton(false);
        director.playableAsset = null;
    }

    void SkipButtonAct()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) // PC 이용의 경우도 있기 때문에 다음과 같은 조건을 부여한다.
        {
            UIManager.instance.ShowSkipButton(true);
        }
    }

    public void Skip()
    {
        if (GameManager.instance.isPause) // Pause 중에 작동이 되는걸 막는다.
            return;

        UIManager.instance.ShowSkipButton(false);

        switch (director.playableAsset.name)
        {
            case "BossAppear":
                BossStageManager.instance.BossStageStart();
                break;
            case "BossDefeat":
                BossStageManager.instance.GameFinish();
                break;
        }
    }
}