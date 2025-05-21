using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;

    private bool updatedDoors = false;
    private bool isPlayerInRoom = false;
    public static RoomController instance;
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
        if (RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();
        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doorType)
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
        if (name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }

            if (Input.GetKeyDown(KeyCode.K))
    {
        Debug.Log("press k");
        foreach (Door door in doors.ToList())
        {
            if (door != null && door.gameObject != null)
            {
                bool isConnected = IsConnectedRoom(door.doorType);
                if (isConnected)
                {
                    door.gameObject.SetActive(true);
                    Debug.Log($"Activated door: {door.doorType} in room {gameObject.name}");
                }
            }
        }
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
            }
        }
    }

    private bool IsConnectedRoom(Door.DoorType doorType)
    {
        switch (doorType)
        {
            case Door.DoorType.right:
                return GetRight() == null;
            case Door.DoorType.left:
                return GetLeft() == null;
            case Door.DoorType.top:
                return GetTop() == null;
            case Door.DoorType.bottom:
                return GetBottom() == null;
            default:
                return false;
        }
    }

    public Room GetRight()
    {
        if (RoomController.instance.DoesRoomExist(X + 1, Y))
            return RoomController.instance.FindRoom(X + 1, Y);
        return null;
    }

    public Room GetLeft()
    {
        if (RoomController.instance.DoesRoomExist(X - 1, Y))
            return RoomController.instance.FindRoom(X - 1, Y);
        return null;
    }

    public Room GetTop()
    {
        if (RoomController.instance.DoesRoomExist(X, Y + 1))
            return RoomController.instance.FindRoom(X, Y + 1);
        return null;
    }

    public Room GetBottom()
    {
        if (RoomController.instance.DoesRoomExist(X, Y - 1))
            return RoomController.instance.FindRoom(X, Y - 1);
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
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered room: " + gameObject.name);
            isPlayerInRoom = true;
            RoomController.instance.OnPlayerEnterRoom(this);
            CheckForEnemies();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited room: " + gameObject.name);
            isPlayerInRoom = false;
        }
    }

    public void CheckForEnemies()
    {
        if (!isPlayerInRoom)
            return;

        bool hasEnemies = false;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy"))
            {
                hasEnemies = true;
                break;
            }
        }

        foreach (Door door in doors.ToList())
        {
            if (door != null && door.gameObject != null)
            {
                bool isConnected = IsConnectedRoom(door.doorType);
                if (isConnected)
                {
                    if (!hasEnemies)
                    {
                        Destroy(door.gameObject);
                        doors.Remove(door);
                    }
                    else
                    {
                        door.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
