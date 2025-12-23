using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    public static MapManager Instance { get; private set; }

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase snowTile;
    [SerializeField] private TileBase iceTile;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private const int SIZE = 5;
    private int[,] mapData = new int[SIZE, SIZE];

    [SerializeField] private GameObject torchPrefab;

    private GameObject currentTorch;
    private int torchSpawnLevel;
    private Vector2Int torchPos;

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
    public void SpawnTorch(int currentLevel)
    {
        if (currentTorch != null) return;

        Vector2Int playerPos = ValueManager.GetPlayerGridPos();
        int mapSizeX = tilemap.size.x;
        int mapSizeY = tilemap.size.y;

        // 플레이어 주변 ±1 후보 수집 (플레이어 위치 제외)
        List<Vector2Int> candidates = new List<Vector2Int>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // 플레이어 위치 제외

                int nx = playerPos.x + dx;
                int ny = playerPos.y + dy;

                if (nx >= 0 && nx < mapSizeX && ny >= 0 && ny < mapSizeY)
                {
                    candidates.Add(new Vector2Int(nx, ny));
                }
            }
        }

        if (candidates.Count == 0) return; // 주변에 스폰할 공간 없으면 종료

        Vector2Int pos = candidates[UnityEngine.Random.Range(0, candidates.Count)];

        Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(pos.x, pos.y, 0))
                           + tilemap.cellSize / 2f;

        currentTorch = Instantiate(torchPrefab, worldPos, Quaternion.identity);
        torchSpawnLevel = currentLevel;
        torchPos = pos;
    }

    public void CheckTorchRemove(int currentLevel)
    {
        if (currentTorch == null) return;

        if (currentLevel - torchSpawnLevel >= 7)
        {
            Destroy(currentTorch);
            currentTorch = null;
        }
    }
    private Vector2Int GetRandomPosition()
    {
        return new Vector2Int(
            Random.Range(0, SIZE),
            Random.Range(0, SIZE)
        );
    }

    public Vector2Int GetTorchPosition()
    {
        if (currentTorch == null)
            return new Vector2Int(-1, -1);

        return torchPos;
    }
    public void RemoveTorch()
    {
        if (currentTorch == null) return;

        Destroy(currentTorch);
        currentTorch = null;
    }
}
