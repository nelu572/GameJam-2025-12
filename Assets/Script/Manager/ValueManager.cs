using UnityEngine;

public class ValueManager : MonoBehaviour
{
    public static readonly Vector2 gridOrigin = new Vector2(-4f, -8.35f);
    public static readonly float cellSize = 2f;

    private static Vector2Int playerGridPos;

    public static Vector2Int GetPlayerGridPos()
    {
        return playerGridPos;
    }

    public static void SetPlayerGridPos(Vector2Int pos)
    {
        playerGridPos = pos;
    }


    public static Vector2 GridToWorld(Vector2Int gridPos)
    {
        return new Vector2(
            gridOrigin.x + gridPos.x * cellSize,
            gridOrigin.y + gridPos.y * cellSize
        );
    }

    public static Vector2Int WorldToGrid(Vector2 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt((worldPos.y - gridOrigin.y) / cellSize);
        return new Vector2Int(x, y);
    }

    private static int mapSize = 5;
    public static int get_mapSize()
    {
        return mapSize;
    }
}
