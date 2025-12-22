using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor.Callbacks;
using UnityEditor.ShaderGraph;
using Unity.Mathematics;

public class SnowBall1 : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;

    [SerializeField] private Sprite snowball;
    [SerializeField] private Sprite arrow;

    private Vector3 beforePos;

    private float jump_height = 1.5f;
    private Vector2Int gridPos;
    private int dir;
    private bool move;
    private Vector2Int targetGridDir;

    private SpriteRenderer sr;

    private Vector2Int spawnGridPos;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>(); // 추가
        beforePos = transform.position;
    }


    void Start()
    {
        float minus = Math.Min(LevelManager.GetNowLevel() / 50f, 0.25f);
        duration -= minus;
        move = false;
    }

    public void set_gridPos(Vector2Int pos)
    {
        gridPos = pos;
        spawnGridPos = pos; // 최초 스폰 위치 저장
    }
    public void set_Dir(int dir)
    {
        this.dir = dir;
    }

    public void drawMoveMark()
    {
        gameObject.SetActive(true);
        sr.sprite = arrow;
        sr.color = new Color(1, 0, 0, 0.85f);
        transform.position += transform.right * 0.65f;
        transform.eulerAngles += new Vector3(0, 0, -90);

    }
    public void deleteMoveMark()
    {
        gameObject.SetActive(false);
        sr.sprite = snowball;
        sr.color = Color.white;
        transform.position = beforePos;
    }


    public void start_move()
    {
        if (move)
            return;

        gameObject.SetActive(true);
        move = true;

        Vector3 startPos = transform.position;

        int mapSize = ValueManager.get_mapSize();
        targetGridDir = Vector2Int.zero;
        switch (dir)
        {
            case 0: // 위 → 아래를 바라봄
                targetGridDir = new Vector2Int(0, -1);
                break;

            case 1: // 아래 → 위를 바라봄
                targetGridDir = new Vector2Int(0, 1);
                break;

            case 2: // 왼쪽 → 오른쪽을 바라봄
                targetGridDir = new Vector2Int(1, 0);
                break;

            case 3: // 오른쪽 → 왼쪽을 바라봄
                targetGridDir = new Vector2Int(-1, 0);
                break;
        }
        Move(0);
    }
    void UpdateSortingOrder()
    {
        Vector2Int playerPos = ValueManager.GetPlayerGridPos();

        if (gridPos.y > playerPos.y)
            sr.sortingLayerName = "BackBullet";   // 플레이어 뒤
        else
            sr.sortingLayerName = "Bullet";  // 플레이어 앞
    }

    public void set_SortingLayer(String Layer)
    {
        sr.sortingLayerName = Layer;
    }

    private void Move(int i)
    {
        float pos_y = transform.position.y;

        Vector2 targetPos = ValueManager.GridToWorld(targetGridDir + gridPos);
        Vector3 endPos = new Vector3(targetPos.x, targetPos.y, 0);

        transform.DOMoveX(endPos.x, duration).SetEase(Ease.Linear);
        transform.DOMoveY((pos_y + endPos.y) / 2f + jump_height, duration / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    if (i >= 5)
                    {
                        SnowBallMove.Instance.DestroySnowball(gameObject, 1);
                        return;
                    }
                    else
                    {
                        transform.DOMoveY(endPos.y, duration / 2f)
                            .SetEase(Ease.InQuad)
                            .OnComplete(() =>
                            {
                                gridPos += targetGridDir;
                                UpdateSortingOrder();
                                PlayerHit.Instance.CheckHit(gridPos);
                                Move(i + 1);
                            });
                    }
                });
    }

    public Vector2Int get_spawnGridPos()
    {
        return spawnGridPos;
    }
}