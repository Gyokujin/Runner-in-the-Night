using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] bullets;
    private List<GameObject>[] pools;
    private List<GameObject> pool;

    void Awake()
    {
        pools = new List<GameObject>[bullets.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        return select;
    }
}