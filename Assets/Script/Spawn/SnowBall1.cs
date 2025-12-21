using UnityEngine;
using DG.Tweening;

public class SnowBall1 : MonoBehaviour
{
    [SerializeField] private float duration = 0f;

    private float jump_height = 1.5f;
    private Vector2Int gridPos;
    private int dir;
    private bool move;
    private Vector2Int targetGridDir;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>(); // 추가
    }


    void Start()
    {
        move = false;
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, pos.z);
    }

    public void set_gridPos(Vector2Int pos)
    {
        gridPos = pos;
    }

    public void set_Dir(int dir)
    {
        this.dir = dir;
    }

    public void start_move()
    {
        if (move)
            return;

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
                        transform.DOKill();
                        SnowBallMove.Instance.DestroySnowball(gameObject, 1);
                        Destroy(gameObject);
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
}