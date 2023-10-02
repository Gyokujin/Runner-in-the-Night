using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Detect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;

            if (transform.parent.GetComponent<E_Chase>())
            {
                transform.parent.GetComponent<E_Chase>().Detect(player);
            }

            gameObject.SetActive(false);
        }
    }
}