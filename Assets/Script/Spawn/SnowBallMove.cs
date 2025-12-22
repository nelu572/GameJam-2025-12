using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SnowBallMove : MonoBehaviour
{
    // 싱글톤
    public static SnowBallMove Instance { get; private set; }

    private bool isMovable = false;
    private int snowballCount = 0;
    private List<GameObject> snowball1 = new List<GameObject>();

    // 스폰 중복 방지
    private HashSet<Vector2Int> usedSpawnPos = new HashSet<Vector2Int>();

    // 안전 행 / 열 (둘 중 하나만 사용)
    private int reservedRow = -1;
    private int reservedCol = -1;

    [SerializeField] private GameObject SnowBall_P1;

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
            snowball.GetComponent<SnowBall1>()?.drawMoveMark();
        }
    }

    public void allSnowBallDeletedMark()
    {
        foreach (GameObject snowball in snowball1)
        {
            snowball.GetComponent<SnowBall1>()?.deleteMoveMark();
        }
    }

    // 스폰 진입점
    public void SpawnSnowball()
    {
        usedSpawnPos.Clear();
        reservedRow = -1;
        reservedCol = -1;

        Vector2Int playerPos = ValueManager.GetPlayerGridPos();

        // 안전 행 또는 열 하나 결정
        bool reserveRowFlag = UnityEngine.Random.value < 0.5f;

        if (reserveRowFlag)
        {
            do
            {
                reservedRow = UnityEngine.Random.Range(0, 5);
            } while (reservedRow == playerPos.y);
        }
        else
        {
            do
            {
                reservedCol = UnityEngine.Random.Range(0, 5);
            } while (reservedCol == playerPos.x);
        }

        int min_plus = (int)MathF.Min(LevelManager.GetNowLevel() / 3f, 3f);
        int max_plus = (int)MathF.Min(LevelManager.GetNowLevel() / 8f, 4f);
        int amount = UnityEngine.Random.Range(2 + min_plus, 3 + max_plus);

        for (int i = 0; i < amount; i++)
        {
            Spawn_Snowball_n1();
        }
    }

    // 개별 눈덩이 스폰
    private void Spawn_Snowball_n1()
    {
        int mapSize = ValueManager.get_mapSize();

        Vector2Int gridPos;
        Quaternion rot;
        int dir;
        int mid;

        const int MAX_TRY = 40;
        int tryCount = 0;

        while (true)
        {
            if (tryCount++ > MAX_TRY)
                return;

            dir = UnityEngine.Random.Range(0, 4);
            mid = UnityEngine.Random.Range(0, 5);

            if (reservedRow != -1)
            {
                if ((dir == 2 || dir == 3) && mid == reservedRow)
                    continue;
            }
            else if (reservedCol != -1)
            {
                if ((dir == 0 || dir == 1) && mid == reservedCol)
                    continue;
            }

            switch (dir)
            {
                case 0: // 위
                    gridPos = new Vector2Int(mid, mapSize);
                    rot = Quaternion.Euler(0f, 0f, -90f);
                    break;

                case 1: // 아래
                    gridPos = new Vector2Int(mid, -1);
                    rot = Quaternion.Euler(0f, 0f, 90f);
                    break;

                case 2: // 좌
                    gridPos = new Vector2Int(-1, mid);
                    rot = Quaternion.Euler(0f, 0f, 0f);
                    break;

                case 3: // 우
                    gridPos = new Vector2Int(mapSize, mid);
                    rot = Quaternion.Euler(0f, 0f, 180f);
                    break;

                default:
                    continue;
            }

            if (usedSpawnPos.Contains(gridPos))
                continue;

            usedSpawnPos.Add(gridPos);

            Vector2 worldPos = ValueManager.GridToWorld(gridPos);
            GameObject sb = Instantiate(SnowBall_P1, worldPos, rot);

            SnowBall1 sn1 = sb.GetComponent<SnowBall1>();
            sn1.set_Dir(dir);
            sn1.set_gridPos(gridPos);

            snowball1.Add(sb);
            snowballCount++;
            return;
        }
    }

    // 눈덩이 제거
    public void DestroySnowball(GameObject snowball, int num)
    {
        SnowBall1 sb = snowball.GetComponent<SnowBall1>();
        if (sb != null)
        {
            usedSpawnPos.Remove(sb.get_spawnGridPos());
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
            snowball.GetComponent<SnowBall1>()?.start_move();
        }
        isMovable = true;
    }

    public void set_allSnowball_SortingLayer(string layer)
    {
        foreach (GameObject snowball in snowball1)
        {
            snowball.GetComponent<SnowBall1>()?.set_SortingLayer(layer);
        }
    }

    public bool get_isMovable()
    {
        return isMovable;
    }
}
