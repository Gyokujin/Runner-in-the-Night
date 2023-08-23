using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float offsetX;

    void Update()
    {
        transform.position = new Vector3(target.position.x + offsetX, transform.position.y, transform.position.z);
    }
}