using System.Data;
using DG.Tweening;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 예정 위치 표시")]
    [SerializeField] private GameObject previewObject;

    private Vector2Int currentDir = Vector2Int.zero;
    private Vector2Int NextPos;
    private bool hasStoredPos = false;

    private float y_offset;

    void Start()
    {
        y_offset = transform.localScale.y / 2 + 0.20f;

        ValueManager.SetPlayerGridPos(new Vector2Int(2, 2));

        Vector2 pos = ValueManager.GridToWorld(ValueManager.GetPlayerGridPos());
        transform.position = new Vector3(pos.x, pos.y + y_offset, 0);
    }

    void Update()
    {
        if (!StateManager.get_canMoving())
            return;

        HandleDirectionToggle();
        UpdateNextPos();
        PlayerValues.set_Dir(currentDir);

        if (hasStoredPos && InputManager.GetConfirmDown())
        {
            Move(NextPos);
            ClearPreview();
            reset_move();
        }

    }

    // 방향 토글 입력
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
        int y = currentDir.y;
        if (x + value < -1 || x + value > 1)
        {
            if (x == 1 && value == 1 || x == -1 && value == -1)
            {
                currentDir.y = 0;
            }
            return;
        }
        currentDir.x = x + value;
    }

    void ToggleY(int value)
    {
        int x = currentDir.x;
        int y = currentDir.y;
        if (y + value < -1 || y + value > 1)
        {
            if (y == 1 && value == 1 || y == -1 && value == -1)
            {
                currentDir.x = 0;
            }
            return;
        }
        currentDir.y = currentDir.y + value;
    }

    // 다음 좌표 계산
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

        if (!previewObject.activeSelf)
            previewObject.SetActive(true);
    }

    void ClearPreview()
    {
        hasStoredPos = false;

        if (previewObject.activeSelf)
            previewObject.SetActive(false);
    }

    // DOTween 점프 이동
    public void Move(Vector2Int target)
    {
        ValueManager.SetPlayerGridPos(target);
        float pos_y = transform.position.y + y_offset;
        Vector2 targetPos = ValueManager.GridToWorld(target);
        Vector3 endPos = new Vector3(targetPos.x, targetPos.y + y_offset, 0);

        float jumpHeight = 1.5f;    // 점프 높이
        float jumpDuration = 0.3f;  // 전체 이동 시간

        // X축 이동 (직선)
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


    // 이동 완료 후 실행
    void OnMoveComplete()
    {
        SnowBallMove.Instance.StartMove();
    }

    public void reset_move()
    {
        StateManager.set_canMoving(false);
        ClearPreview();
        currentDir = Vector2Int.zero;
        hasStoredPos = false;
    }

    bool IsMovable(Vector2Int gridPos)
    {
        int mapSize = ValueManager.get_mapSize();

        if (gridPos.x < 0 || gridPos.x >= mapSize)
            return false;
        if (gridPos.y < 0 || gridPos.y >= mapSize)
            return false;

        return true;
    }
}
