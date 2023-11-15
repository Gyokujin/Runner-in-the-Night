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
        BossAppear
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

    public IEnumerator BossEvent()
    {
        PlayTimeLine(Timeline.Danger);

        yield return new WaitForSeconds((float)timelines[(int)Timeline.Danger].duration);
        PlayTimeLine(Timeline.BossAppear);
    }

    public void PlayTimeLine(Timeline index)
    {
        director.playableAsset = timelines[(int)index];
        director.Play();
        Debug.Log(index + "Ω««‡¡ﬂ");
    }
}