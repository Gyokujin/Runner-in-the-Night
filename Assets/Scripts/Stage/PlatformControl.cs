using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private GameObject[] platforms;
    private int moveIndex = 0;

    [SerializeField]
    private float minXPos;
    private float lastYPos;
    [SerializeField]
    private float transferX;
    [SerializeField]
    private float transferYMin;
    [SerializeField]
    private float transferYMax;
    [SerializeField]
    private float transferYLimit;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        platforms = new GameObject[transform.childCount];

        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i] = transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (platforms[moveIndex].transform.position.x < minXPos)
        {
            PlatformTransfer(platforms[moveIndex]);
        }
    }

    void PlatformTransfer(GameObject platform)
    {
        float transferY = Random.Range(Mathf.Max(lastYPos - transferYLimit, transferYMin), Mathf.Min(lastYPos  + transferYLimit, transferYMax));
        lastYPos = transferY;
        platform.transform.position = new Vector2(transferX, transferY);
        moveIndex = (moveIndex + 1) % (platforms.Length);
    }
}