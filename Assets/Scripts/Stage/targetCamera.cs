using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float offsetX;
    private bool onAct = true;

    void Update()
    {
        if (onAct)
        {
            transform.position = new Vector3(target.position.x + offsetX, transform.position.y, transform.position.z);
        }
    }

    public void ActCamera()
    {
        onAct = true;
    }

    public void StopCamera()
    {
        onAct = false;
    }
}