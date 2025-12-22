using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase snowTile;
    [SerializeField] private TileBase iceTile;

    private const int SIZE = 5;
    private int[,] mapData = new int[SIZE, SIZE];

    void Start()
    {
        InitMapData();
        DrawMap();
    }

    private void InitMapData()
    {
        for (int x = 0; x < SIZE; x++)  
        {
            for (int y = 0; y < SIZE; y++)
            {
                mapData[x, y] = 0;
            }
        }
    }

    private void DrawMap()
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, mapData[x, y] == 0 ? snowTile : iceTile);
            }
        }
    }

    public void SetTileState(Vector2Int pos, int state)
    {
        if (!IsInBounds(pos)) return;

        mapData[pos.x, pos.y] = state;
        tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), state == 0 ? snowTile : iceTile);
    }

    public int GetTileState(Vector2Int pos)
    {
        if (!IsInBounds(pos)) return 0;
        return mapData[pos.x, pos.y];
    }

    public void SetAround3x3Ice(Vector2Int center)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                Vector2Int pos = center + new Vector2Int(dx, dy);
                if (!IsInBounds(pos)) continue;

                mapData[pos.x, pos.y] = 2; // ice
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), iceTile);
            }
        }
    }

    public void DecreaseAllPositiveStates()
    {
        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                if (mapData[x, y] > 0)
                {
                    mapData[x, y]--;

                    tilemap.SetTile(new Vector3Int(x, y, 0),
                        mapData[x, y] == 0 ? snowTile : iceTile);
                }
            }
        }
    }

    public bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < SIZE && pos.y >= 0 && pos.y < SIZE;
    }
}
