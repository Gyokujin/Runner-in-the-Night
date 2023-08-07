using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public enum LoopType { Scroll, Relocation }
    public LoopType loopType;

    [SerializeField]
    private float targetPosX; // 이동할 X 좌표

    void Awake()
    {
        if (loopType == LoopType.Scroll)
        {
            targetPosX = GetComponent<BoxCollider2D>().size.x;
        }
    }

    void Update()
    {
        if (transform.position.x <= -targetPosX)
        {
            Reposition();
        }
    }

    void Reposition()
    {
        transform.position = new Vector2(targetPosX, transform.position.y);
    }
}