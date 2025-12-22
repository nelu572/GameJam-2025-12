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
            LevelManager.NextLevel();
        }
    }
    public void allSnowBallDrawMark()
    {
        foreach (GameObject snowball in snowball1)
        {
            SnowBall1 sbScript = snowball.GetComponent<SnowBall1>();
            sbScript.drawMoveMark();
        }
    }
    public void allSnowBallDeletedMark()
    {
        foreach (GameObject snowball in snowball1)
        {
            SnowBall1 sbScript = snowball.GetComponent<SnowBall1>();
            sbScript.deleteMoveMark();
        }
    }

    public void SpawnSnowball()
    {
        int min_plus = (int)MathF.Min((float)LevelManager.GetNowLevel() / 3.0f, 3.0f);
        int max_plus = (int)MathF.Min((float)LevelManager.GetNowLevel() / 8.0f, 4.0f);
        int amount = UnityEngine.Random.Range(2 + min_plus, 3 + max_plus);

        for (int i = 0; i < amount; i++)
        {
            Spawn_Snowball_n1();
        }
    }

    private void Spawn_Snowball_n1()
    {
        int mapSize = ValueManager.get_mapSize();

        Vector2Int gridPos = Vector2Int.zero;
        Quaternion rot = Quaternion.identity;
        int dir = 0;

        const int MAX_TRY = 20;
        int tryCount = 0;

        // 중복되지 않은 위치 나올 때까지 재시도
        do
        {
            if (tryCount++ > MAX_TRY)
                return; // 공간 없으면 그냥 포기

            dir = UnityEngine.Random.Range(0, 4);
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

        } while (usedSpawnPos.Contains(gridPos));

        // 위치 사용 등록
        usedSpawnPos.Add(gridPos);

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
        SnowBall1 sb = snowball.GetComponent<SnowBall1>();
        if (sb != null)
        {
            usedSpawnPos.Remove(sb.get_spawnGridPos()); // 위치 해제
        }

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
