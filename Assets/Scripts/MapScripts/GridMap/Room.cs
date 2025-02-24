using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;

    private bool updatedDoors = false;
    private bool isPlayerInRoom = false;
    private bool doorsRemoved = false;

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    public List<Door> doors = new List<Door>();
    
    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();
        foreach(Door d in ds)
        {
            doors.Add(d);
            switch(d.doorType)
            {
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }

        RoomController.instance.RegisterRoom(this);
        RemoveUnconnectedDoors();
    }

    void Update()
    {
        if(name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null && door.gameObject != null)
            {
                bool shouldRemove = !IsConnectedRoom(door.doorType);
                door.gameObject.SetActive(!shouldRemove);
                if (shouldRemove)
                {
                    Debug.Log($"Removed unconnected door: {door.doorType} in room {gameObject.name}");
                }
            }
        }
        doorsRemoved = true;
    }

    private bool IsConnectedRoom(Door.DoorType doorType)
    {
        switch (doorType)
        {
            case Door.DoorType.right:
                return GetRight() != null;
            case Door.DoorType.left:
                return GetLeft() != null;
            case Door.DoorType.top:
                return GetTop() != null;
            case Door.DoorType.bottom:
                return GetBottom() != null;
            default:
                return false;
        }
    }

    public Room GetRight()
    {
        if(RoomController.instance.DoesRoomExist(X + 1, Y))
        {
            return RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }
    
    public Room GetLeft()
    {
        if(RoomController.instance.DoesRoomExist(X - 1, Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;
    }
    
    public Room GetTop()
    {
        if(RoomController.instance.DoesRoomExist(X, Y + 1))
        {
            return RoomController.instance.FindRoom(X, Y + 1);
        }
        return null;
    }
    
    public Room GetBottom()
    {
        if(RoomController.instance.DoesRoomExist(X, Y - 1))
        {
            return RoomController.instance.FindRoom(X, Y - 1);
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * Width, Y * Height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player entered room: " + gameObject.name);
            isPlayerInRoom = true;
            RoomController.instance.OnPlayerEnterRoom(this);
            CheckForEnemies();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player exited room: " + gameObject.name);
            isPlayerInRoom = false;
        }
    }

    public void CheckForEnemies()
    {
        if (!isPlayerInRoom)
        {
            Debug.Log("Player not in this room, skipping enemy check: " + gameObject.name);
            return;
        }
    
        List<GameObject> enemiesInRoom = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemiesInRoom.Add(child.gameObject);
            }
        }
    
        if (enemiesInRoom.Count <= 0)
        {
            Debug.Log("No enemies in room: " + gameObject.name);
            RemoveDoors();
        }
        else
        {
            Debug.Log(enemiesInRoom.Count + " enemies in room: " + gameObject.name);
            SpawnDoors();
        }
    }

    private void SpawnDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null && door.gameObject != null)
            {
                bool shouldSpawn = IsConnectedRoom(door.doorType);
                door.gameObject.SetActive(shouldSpawn);
                if (shouldSpawn)
                {
                    Debug.Log($"Spawned door: {door.doorType} in room {gameObject.name}");
                }
            }
        }
    }

    private void RemoveDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null && door.gameObject != null)
            {
                door.gameObject.SetActive(false);
                Debug.Log($"Removed door: {door.doorType} in room {gameObject.name}");
            }
        }
    }
}
