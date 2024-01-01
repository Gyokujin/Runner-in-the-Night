using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour
{
    public float spikePosY;
    public float[] enemyPosY;

    public int SelectEnemy()
    {
        int kind = 0;
        int num;
        int maxNum;

        if (GameManager.instance.score < 200) // Guardian = 0123, Diver = 345, Chaser = 67, Wanderer = 8, Blazer = 9
        {
            maxNum = 6;
        }
        else if (GameManager.instance.score < 500)
        {
            maxNum = 8;
        }
        else if (GameManager.instance.score < 900)
        {
            maxNum = 9;
        }
        else
        {
            maxNum = 10;
        }

        num = Random.Range(0, maxNum);

        switch (num)
        {
            case 0:
            case 1:
            case 2:
                kind = 0;
                break;

            case 3:
            case 4:
            case 5:
                kind = 1;
                break;

            case 6:
            case 7:
                kind = 2;
                break;

            case 8:
                kind = 3;
                break;

            case 9:
                kind = 4;
                break;
        }

        return kind;
    }
}