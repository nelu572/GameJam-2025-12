using System.Data;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 예정 위치 표시")]
    [SerializeField] private GameObject previewObject;

    private Vector2Int currentDir = Vector2Int.zero;
    private Vector2Int NextPos;
    private bool hasStoredPos = false;

    private float y_offset;

    private float 
    void Start()
    {
        y_offset = transform.localScale.y / 2;

        ValueManager.SetPlayerGridPos(Vector2Int.zero);

        Vector2 pos = ValueManager.GridToWorld(ValueManager.GetPlayerGridPos());
        transform.position = new Vector3(pos.x, pos.y + y_offset, 0);
    }

    void Update()
    {
        if (!StateManager.get_canMoving())
            return;

        HandleDirectionToggle();
        UpdateNextPos();

        if (hasStoredPos && InputManager.GetConfirmDown())
        {
            Move(NextPos);
            ClearPreview();
        }
    }

    // 방향 토글 입력
    void HandleDirectionToggle()
    {
        if (Input.GetKeyDown(KeyCode.W)) ToggleY(1);
        if (Input.GetKeyDown(KeyCode.S)) ToggleY(-1);
        if (Input.GetKeyDown(KeyCode.D)) ToggleX(1);
        if (Input.GetKeyDown(KeyCode.A)) ToggleX(-1);
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

    //다음 좌표 계산
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

        if (previewObject != null && previewObject.activeSelf)
            previewObject.SetActive(false);
    }

    void Move(Vector2Int target)
    {
        StateManager.set_canMoving(false);

        ValueManager.SetPlayerGridPos(target);
        Vector2 pos = ValueManager.GridToWorld(target);
        transform.position = new Vector3(pos.x, pos.y + y_offset, 0);

        SnowBallMove.start_move();
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
