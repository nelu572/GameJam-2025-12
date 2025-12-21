using System.Data;
using DG.Tweening;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 예정 위치 표시")]
    [SerializeField] private GameObject previewObject;

    [SerializeField] private MapManager mapManager;

    private Vector2Int currentDir = Vector2Int.zero;
    private Vector2Int NextPos;
    private Vector2Int lastMoveDir;

    private bool hasStoredPos = false;
    private bool ice_skill = false;

    private float y_offset;

    void Start()
    {
        y_offset = transform.localScale.y / 2 + 0.20f;

        ValueManager.SetPlayerGridPos(new Vector2Int(2, 2));

        Vector2 pos = ValueManager.GridToWorld(ValueManager.GetPlayerGridPos());
        transform.position = new Vector3(pos.x, pos.y + y_offset, 0);

        ice_skill = true;
    }

    void Update()
    {
        if (!StateManager.get_canMoving()) return;

        if (ice_skill && InputManager.GetShiftDown())
        {
            ice_skill = false;
            Vector2Int playerPos = ValueManager.GetPlayerGridPos();
            mapManager.SetAround3x3Ice(playerPos);
        }

        HandleDirectionToggle();
        UpdateNextPos();
        PlayerValues.set_Dir(currentDir);

        if (hasStoredPos && InputManager.GetConfirmDown())
        {
            lastMoveDir = currentDir;
            Move(NextPos);
            ClearPreview();
            reset_move();
        }
    }

    void HandleDirectionToggle()
    {
        if (InputManager.GetKey(KeyCode.W)) ToggleY(1);
        if (InputManager.GetKey(KeyCode.S)) ToggleY(-1);
        if (InputManager.GetKey(KeyCode.D)) ToggleX(1);
        if (InputManager.GetKey(KeyCode.A)) ToggleX(-1);
    }

    void ToggleX(int value)
    {
        int x = currentDir.x;
        if (x + value < -1 || x + value > 1)
        {
            if (x == 1 && value == 1 || x == -1 && value == -1) currentDir.y = 0;
            return;
        }
        currentDir.x = x + value;
    }

    void ToggleY(int value)
    {
        int y = currentDir.y;
        if (y + value < -1 || y + value > 1)
        {
            if (y == 1 && value == 1 || y == -1 && value == -1) currentDir.x = 0;
            return;
        }
        currentDir.y = y + value;
    }

    void UpdateNextPos()
    {
        if (currentDir == Vector2Int.zero)
        {
            ClearPreview();
            return;
        }

        Vector2Int current = ValueManager.GetPlayerGridPos();
        Vector2Int tempPos = current + currentDir;

        if (IsMovable(tempPos))
        {
            NextPos = tempPos;
            hasStoredPos = true;
            UpdatePreview(tempPos);
        }
        else
        {
            currentDir = Vector2Int.zero;
            ClearPreview();
        }
    }

    void UpdatePreview(Vector2Int gridPos)
    {
        Vector2 pos = ValueManager.GridToWorld(gridPos);
        previewObject.transform.position = new Vector3(pos.x, pos.y, 0);

        if (!previewObject.activeSelf) previewObject.SetActive(true);
    }

    public void ClearPreview()
    {
        hasStoredPos = false;
        if (previewObject.activeSelf) previewObject.SetActive(false);
    }

    public void Move(Vector2Int target)
    {
        ValueManager.SetPlayerGridPos(target);
        float pos_y = transform.position.y + y_offset;
        Vector2 targetPos = ValueManager.GridToWorld(target);
        Vector3 endPos = new Vector3(targetPos.x, targetPos.y + y_offset, 0);

        float jumpHeight = 1.5f;
        float jumpDuration = 0.3f;

        transform.DOMoveX(endPos.x, jumpDuration).SetEase(Ease.Linear);
        transform.DOMoveY((pos_y + endPos.y) / 2f + jumpHeight, jumpDuration / 2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOMoveY(endPos.y, jumpDuration / 2f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        OnMoveComplete();
                    });
            });
    }

    void OnMoveComplete()
    {
        Vector2Int currentPos = ValueManager.GetPlayerGridPos();

        // 현재 밟고 있는 타일이 얼음이면 등속 이동
        if (mapManager.GetTileState(currentPos) > 0)
        {
            Vector2Int nextPos = currentPos + lastMoveDir;

            if (!mapManager.IsInBounds(nextPos))
            {
                SnowBallMove.Instance.StartMove();
                return;
            }

            MoveIce(nextPos); // 등속 이동
            return;
        }

        // 일반 타일이면 멈춤
        SnowBallMove.Instance.StartMove();
    }

    void MoveIce(Vector2Int target)
    {
        Vector2 targetPos = ValueManager.GridToWorld(target);
        Vector3 endPos = new Vector3(targetPos.x, targetPos.y + y_offset, 0);

        // 현재 위치와 목표 위치 거리 계산
        float distance = Vector3.Distance(transform.position, endPos);

        float speed = 6f; // 유닛/초
        float duration = distance / speed; // 대각선 거리 보정 포함

        transform.DOMove(endPos, duration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 이동 완료 후 좌표 갱신
                ValueManager.SetPlayerGridPos(target);
                // 밟은 타일이 얼음이면 반복
                OnMoveComplete();
            });
    }

    public void reset_move()
    {
        StateManager.set_canMoving(false);
        ClearPreview();
        currentDir = Vector2Int.zero;
        hasStoredPos = false;
        ice_skill = true;
    }

    bool IsMovable(Vector2Int gridPos)
    {
        int mapSize = ValueManager.get_mapSize();
        if (gridPos.x < 0 || gridPos.x >= mapSize) return false;
        if (gridPos.y < 0 || gridPos.y >= mapSize) return false;
        return true;
    }
}
