using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Detect : MonoBehaviour
{
    private bool onDetect = false;
    public Vector2 targetPos;

    void OnDisable()
    {
        onDetect = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!onDetect && collision.gameObject.CompareTag("Player"))
        {
            onDetect = true;
            targetPos = collision.gameObject.transform.position;
        }
    }
}