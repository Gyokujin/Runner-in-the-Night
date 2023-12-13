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
        Danger,
        BossAppear,
        BossDefeat
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

    public IEnumerator DangerEvent()
    {
        PlayTimeLine(Timeline.Danger);
        yield return new WaitForSeconds((float)timelines[(int)Timeline.Danger].duration);
    }

    public IEnumerator BossEvent()
    {
        PlayTimeLine(Timeline.BossAppear);
        yield return new WaitForSeconds((float)timelines[(int)Timeline.BossAppear].duration);
    }

    public IEnumerator BossDefeat()
    {
        PlayTimeLine(Timeline.BossDefeat);
        yield return null;
    }

    public void PlayTimeLine(Timeline index)
    {
        director.playableAsset = timelines[(int)index];
        director.Play();
    }
}