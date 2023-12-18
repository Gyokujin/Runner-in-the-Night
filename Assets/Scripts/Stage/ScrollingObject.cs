using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    void Update()
    {
        if (!GameManager.instance.isGameOver && GameManager.instance.isLive)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}