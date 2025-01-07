using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemyPlacer : MonoBehaviour
{
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public List<GameObject> enemyPrefabs;
    public int minEnemiesPerRoom = 1;
    public int maxEnemiesPerRoom = 3;

    public void PlaceEnemies(BoundsInt roomBounds)
    {
        int enemiesToPlace = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);
        int enemiesPlaced = 0;
        int attempts = 0;
        int maxAttempts = 50;

        while (enemiesPlaced < enemiesToPlace && attempts < maxAttempts)
        {
            Vector2Int randomPosition = GetRandomPositionInRoom(roomBounds);

            if (IsValidPlacementPosition(randomPosition))
            {
                GameObject enemyToPlace = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                Vector3 worldPosition = floorTilemap.GetCellCenterWorld((Vector3Int)randomPosition);
                Instantiate(enemyToPlace, worldPosition, Quaternion.identity);
                enemiesPlaced++;
            }

            attempts++;
        }
    }

    private Vector2Int GetRandomPositionInRoom(BoundsInt roomBounds)
    {
        int x = Random.Range(roomBounds.xMin, roomBounds.xMax);
        int y = Random.Range(roomBounds.yMin, roomBounds.yMax);
        return new Vector2Int(x, y);
    }

    private bool IsValidPlacementPosition(Vector2Int position)
    {
        return floorTilemap.HasTile((Vector3Int)position) && !wallTilemap.HasTile((Vector3Int)position);
    }
}
