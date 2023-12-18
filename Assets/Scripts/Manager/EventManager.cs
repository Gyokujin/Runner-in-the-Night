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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

        director = GetComponent<PlayableDirector>();
    }

    public void PlayTimeLine(Timeline index)
    {
        director.playableAsset = timelines[(int)index];
        director.Play();
    }
}