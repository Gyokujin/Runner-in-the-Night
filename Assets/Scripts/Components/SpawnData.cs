using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour
{
    public int SelectEnemy()
    {
        int kind;

        if (GameManager.instance.score < 100)
        {
            kind = Random.Range(0, 2);
        }
        else if (GameManager.instance.score < 300)
        {
            kind = Random.Range(0, 3);
        }
        else if (GameManager.instance.score < 600)
        {
            kind = Random.Range(0, 4);
        }
        else
        {
            kind = Random.Range(0, 5);
        }

        return kind;
    }
}

public struct SpawnDamage
{

}