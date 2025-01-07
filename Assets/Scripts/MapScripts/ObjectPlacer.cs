using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public List<GameObject> objectPrefabs;
    public int maxObjectsPerRoom = 5;

    public void PlaceObjects(BoundsInt roomBounds)
    {
        int objectsPlaced = 0;
        int attempts = 0;
        int maxAttempts = 50;

        while (objectsPlaced < maxObjectsPerRoom && attempts < maxAttempts)
        {
            Vector2Int randomPosition = GetRandomPositionInRoom(roomBounds);

            if (IsValidPlacementPosition(randomPosition))
            {
                GameObject objectToPlace = objectPrefabs[Random.Range(0, objectPrefabs.Count)];
                Vector3 worldPosition = floorTilemap.GetCellCenterWorld((Vector3Int)randomPosition);
                Instantiate(objectToPlace, worldPosition, Quaternion.identity);
                objectsPlaced++;
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
