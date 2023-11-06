using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField]
    private float minXPos;
    [SerializeField]
    private float transferX;

    void Update()
    {
        if (transform.position.x <= minXPos)
        {
            PlatformTransfer();
        }
    }

    void PlatformTransfer()
    {
        transform.position = new Vector2(transferX, transform.position.y);
    }
}