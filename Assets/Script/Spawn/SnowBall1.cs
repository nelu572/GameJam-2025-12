using UnityEngine;
using DG.Tweening;

public class SnowBall1 : MonoBehaviour
{
    [SerializeField] private float duration = 0.75f;

    private Vector2Int gridPos;
    private int dir;
    private bool move;

    void Start()
    {
        move = false;
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
        Vector2Int targetGridPos = Vector2Int.zero;
        switch (dir)
        {
            case 0: // 위 → 아래를 바라봄
                targetGridPos = new Vector2Int(gridPos.x, -1);
                break;

            case 1: // 아래 → 위를 바라봄
                targetGridPos = new Vector2Int(gridPos.x, mapSize);
                break;

            case 2: // 왼쪽 → 오른쪽을 바라봄
                targetGridPos = new Vector2Int(mapSize, gridPos.y);
                break;

            case 3: // 오른쪽 → 왼쪽을 바라봄
                targetGridPos = new Vector2Int(-1, gridPos.y);
                break;
        }
        Vector3 targetPos = ValueManager.GridToWorld(targetGridPos);

        transform.DOMove(targetPos, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    SnowBallMove.Instance.DestroySnowball(gameObject, 1);
                    Destroy(gameObject);
                });
    }
}