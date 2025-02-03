using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    public GameObject playerSpawner;
    public GameObject bossEntrancePrefab;
    public GameObject chestPrefab;
    private bool entraceCanSpawn = true;
    public List<GameObject> enemies;
    
    NavMeshGenerate navMeshGenerate;


    public void Start()
    {
        navMeshGenerate = GetComponent<NavMeshGenerate>();
        RunProceduralGeneration();
        navMeshGenerate.GenerateNavmesh();

    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }
        

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            if(entraceCanSpawn)
            {
                Instantiate(bossEntrancePrefab, new Vector3(roomCenter.x, roomCenter.y, 0f), transform.rotation);
                entraceCanSpawn = false;
            }

            List<Vector2Int> validFloorPositions = new List<Vector2Int>();

            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && 
                   position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                    validFloorPositions.Add(position);
                }
            }

            int enemiesToSpawn = Random.Range(1, 1); // Spawn enemies per room
            for (int j = 0; j < enemiesToSpawn; j++)
            {
                if (validFloorPositions.Count > 0 && enemies.Count > 0)
                {
                    int randomPositionIndex = Random.Range(0, validFloorPositions.Count);
                    Vector2Int spawnPosition = validFloorPositions[randomPositionIndex];
                    int randomEnemyIndex = Random.Range(0, enemies.Count);

                    Instantiate(enemies[randomEnemyIndex], new Vector3(spawnPosition.x, spawnPosition.y, 0f), Quaternion.identity);

                    validFloorPositions.RemoveAt(randomPositionIndex);
                }
            }

            if (Random.Range(0, 100) < 20) // 20% chance
            {
                Vector2Int chestPosition = validFloorPositions[Random.Range(0, validFloorPositions.Count)];
                Instantiate(chestPrefab, new Vector3(chestPosition.x, chestPosition.y, 0f), transform.rotation);
            }
        }
        return floor;
    }


    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        playerSpawner.transform.position = new Vector3(closest.x, closest.y, 0f);
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}