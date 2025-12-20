using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tile;

    void Start()
    {
        tilemap.SetTile(Vector3Int.zero, tile);
    }
}
