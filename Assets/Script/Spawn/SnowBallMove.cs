using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SnowBallMove : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SnowBallMove Instance { get; private set; }

    private bool isMovable = false;
    private int snowballCount = 0;
    private List<GameObject> snowball1 = new List<GameObject>();

    private HashSet<Vector2Int> usedSpawnPos = new HashSet<Vector2Int>();

    [SerializeField] private GameObject SnowBall_P1; // Inspector에서 연결

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (!isMovable) return;

        if (snowballCount <= 0)
        {
            isMovable = false;
            StateManager.set_canMoving(true);
            LevelManager.NextLevel();
        }
    }

    public void SpawnSnowball()
    {
        int plus = LevelManager.GetNowLevel() / 5;
        if (plus > 5)
            plus = 5;
        int amount = UnityEngine.Random.Range(2 + plus, 5 + plus);



        for (int i = 0; i < amount; i++)
        {
            Spawn_Snowball_n1();
        }
    }

    private void Spawn_Snowball_n1()
    {
        int mapSize = ValueManager.get_mapSize();
        int dir = UnityEngine.Random.Range(0, 4);
        Vector2Int gridPos = Vector2Int.zero;
        Quaternion rot = Quaternion.identity;

        int mid = UnityEngine.Random.Range(0, 5);

        switch (dir)
        {
            case 0:
                gridPos = new Vector2Int(mid, mapSize);
                rot = Quaternion.Euler(0f, 0f, -90f);
                break;
            case 1:
                gridPos = new Vector2Int(mid, -1);
                rot = Quaternion.Euler(0f, 0f, 90f);
                break;
            case 2:
                gridPos = new Vector2Int(-1, mid);
                rot = Quaternion.Euler(0f, 0f, 0f);
                break;
            case 3:
                gridPos = new Vector2Int(mapSize, mid);
                rot = Quaternion.Euler(0f, 0f, 180f);
                break;
        }

        Vector2 worldPos = ValueManager.GridToWorld(gridPos);
        GameObject sb = Instantiate(SnowBall_P1, worldPos, rot);

        SnowBall1 sn1 = sb.GetComponent<SnowBall1>();
        sn1.set_Dir(dir);
        sn1.set_gridPos(gridPos);

        snowball1.Add(sb);
        snowballCount++;
    }


    // Snowball 제거
    public void DestroySnowball(GameObject snowball, int num)
    {
        snowball.transform.DOKill();
        if (num == 1)
        {
            snowball1.Remove(snowball);
        }
        snowballCount--;
        Destroy(snowball);
    }

    // 이동 시작
    public void StartMove()
    {
        foreach (GameObject snowball in snowball1)
        {
            SnowBall1 sbScript = snowball.GetComponent<SnowBall1>();
            if (sbScript != null)
                sbScript.start_move();
        }
        isMovable = true;
    }

    // public void allDestroySnowBall()
    // {
    //     for (int i = 0; i < snowball1.Count; i++)
    //     {
    //         DestroySnowball(snowball1[0], 1);
    //     }
    // }

    public void set_allSnowball_SortingLayer(String Layer)
    {
        foreach (GameObject snowball in snowball1)
        {
            SnowBall1 sbScript = snowball.GetComponent<SnowBall1>();
            sbScript.set_SortingLayer(Layer);
        }
    }

    public bool get_isMovable()
    {
        return isMovable;
    }
}
