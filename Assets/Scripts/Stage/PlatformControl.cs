using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    public enum PlatformType
    {
        Random,
        Line
    }

    public PlatformType platformType;

    [SerializeField]
    private GameObject[] randomPlatforms;
    [SerializeField]
    private GameObject[] linePlatforms;

    [Header("PlatformData")]
    private int moveIndex = 0;
    [SerializeField]
    private float[] spawnPosX;
    private SpawnData spawnData;

    [Header("RandomPlatform")]
    [SerializeField]
    private float randomPlatformMinX;
    private float lastYPos;
    [SerializeField]
    private float randomPlatfromTransferX;
    [SerializeField]
    private float randomPlatfromYMin;
    [SerializeField]
    private float randomPlatfromYMax;
    [SerializeField]
    private float randomPlatfromYLimit;

    [Header("LinePlatform")]
    [SerializeField]
    private float linePlatformMinX;
    [SerializeField]
    private float linePlatfromTransferX;

    void Awake()
    {
        if (platformType == PlatformType.Random)
        {
            spawnData = GetComponent<SpawnData>();
        }
    }

    void Update()
    {
        if ((platformType == PlatformType.Random && randomPlatforms[moveIndex].transform.position.x < randomPlatformMinX) || 
            (platformType == PlatformType.Line && linePlatforms[moveIndex].transform.position.x < linePlatformMinX))
        {
            PlatformTransfer(moveIndex);
        }
    }

    void PlatformTransfer(int platformIndex)
    {
        if (this.platformType == PlatformType.Random)
        {
            GameObject platform = randomPlatforms[platformIndex];
            float transferY = Random.Range(Mathf.Max(lastYPos - randomPlatfromYLimit, randomPlatfromYMin), Mathf.Min(lastYPos + randomPlatfromYLimit, randomPlatfromYMax));
            lastYPos = transferY;
            platform.transform.position = new Vector2(randomPlatfromTransferX, transferY);
            PlatformSetting(platformIndex);
            moveIndex = (moveIndex + 1) % (randomPlatforms.Length);
        }
        else if (this.platformType == PlatformType.Line)
        {
            GameObject platform = linePlatforms[platformIndex];
            platform.transform.position = new Vector2(linePlatfromTransferX, platform.transform.position.y);
            moveIndex = (moveIndex + 1) % (linePlatforms.Length);
        }
    }

    public void PlatformChange()
    {
        if (platformType == PlatformType.Random) // 현재 플랫폼타입에서 다른 타입으로 변형한다.
        {
            platformType = PlatformType.Line;

            foreach (GameObject platform in linePlatforms)
            {
                platform.SetActive(true);
            }

            foreach (GameObject platform in randomPlatforms)
            {
                platform.SetActive(false);
            }
        }
        else
        {
            platformType = PlatformType.Random;

            foreach (GameObject platform in randomPlatforms)
            {
                platform.SetActive(true);
            }

            foreach (GameObject platform in linePlatforms)
            {
                platform.SetActive(false);
            }
        }

        moveIndex = 0; // 발판 이동 순번을 초기화한다.
    }

    void PlatformSetting(int platformIndex)
    {
        for (int i = 0; i < spawnPosX.Length; i++)
        {
            GameObject spawnObject = null;
            int spawnType = Random.Range(0, 6); // 0 : 장애물, 1, 2 : 몬스터

            switch (spawnType)
            {
                case 0:
                    spawnObject = PoolManager.instance.Get(PoolManager.PoolType.Obstacle, 0);
                    spawnObject.transform.parent = randomPlatforms[platformIndex].transform;
                    spawnObject.transform.localPosition = new Vector2(spawnPosX[i], spawnData.spikePosY);
                    break;

                case 1:
                case 2:
                case 3:
                    int enemyKind = spawnData.SelectEnemy();
                    spawnObject = PoolManager.instance.Get(PoolManager.PoolType.Enemy, enemyKind);
                    spawnObject.transform.parent = randomPlatforms[platformIndex].transform;
                    spawnObject.transform.localPosition = new Vector2(spawnPosX[i], spawnData.enemyPosY[enemyKind]);
                    break;
            }
        }
    }
}