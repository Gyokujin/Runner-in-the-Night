using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    public void DeAct()
    {
        targetObject.SetActive(false);
    }
}