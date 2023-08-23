using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private GameObject[] obstacles;

    void OnEnable()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (Random.Range(0, 3) == 0)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        }
    }
}