using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase snowTile;
    [SerializeField] private TileBase iceTile;
    [SerializeField] private GameObject torchPrefab;

    private const int SIZE = 5;
    private int[,] mapData = new int[SIZE, SIZE];

    [System.Serializable]
    private class TorchData
    {
        public GameObject obj;
        public Vector2Int pos;
        public int spawnLevel;
    }

    private List<TorchData> torches = new List<TorchData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

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
                tilemap.SetTile(new Vector3Int(x, y, 0),
                    mapData[x, y] == 0 ? snowTile : iceTile);
            }
        }
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

                mapData[pos.x, pos.y] = 2;
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

    // 횃불 생성 (5턴마다 LevelManager에서 호출)
    public void SpawnTorch(int currentLevel)
    {
        Vector2Int playerPos = ValueManager.GetPlayerGridPos();
        List<Vector2Int> candidates = new List<Vector2Int>();

        // 플레이어 주변 +-1
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                Vector2Int pos = playerPos + new Vector2Int(dx, dy);

                // 플레이어 위치 제외
                if (pos == playerPos)
                    continue;

                // 맵 범위 체크
                if (!IsInBounds(pos))
                    continue;

                // 이미 횃불 있는 칸 제외
                if (HasTorchAt(pos))
                    continue;

                candidates.Add(pos);
            }
        }

        // 생성할 수 있는 칸이 없으면 생성 안 함
        if (candidates.Count == 0)
            return;

        Vector2Int spawnPos = candidates[Random.Range(0, candidates.Count)];

        Vector3 worldPos = tilemap.CellToWorld(
            new Vector3Int(spawnPos.x, spawnPos.y, 0)
        ) + tilemap.cellSize / 2f;

        GameObject obj = Instantiate(torchPrefab, worldPos, Quaternion.identity);

        torches.Add(new TorchData
        {
            obj = obj,
            pos = spawnPos,
            spawnLevel = currentLevel
        });
    }
    private bool HasTorchAt(Vector2Int pos)
    {
        for (int i = 0; i < torches.Count; i++)
        {
            if (torches[i].pos == pos)
                return true;
        }
        return false;
    }


    // 7턴 지난 횃불 자동 삭제
    public void CheckTorchRemove(int currentLevel)
    {
        for (int i = torches.Count - 1; i >= 0; i--)
        {
            if (currentLevel - torches[i].spawnLevel >= 7)
            {
                Destroy(torches[i].obj);
                torches.RemoveAt(i);
            }
        }
    }

    // 플레이어가 밟은 횃불 제거
    public bool TryPickupTorch(Vector2Int playerPos)
    {
        for (int i = 0; i < torches.Count; i++)
        {
            if (torches[i].pos == playerPos)
            {
                Destroy(torches[i].obj);
                torches.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    private Vector2Int GetRandomPosition()
    {
        return new Vector2Int(
            Random.Range(0, SIZE),
            Random.Range(0, SIZE)
        );
    }

    public bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < SIZE && pos.y >= 0 && pos.y < SIZE;
    }
}
