using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Detect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (transform.parent.GetComponent<E_Chase>())
            {
                Debug.Log("Check");
            }
        }
    }
}
