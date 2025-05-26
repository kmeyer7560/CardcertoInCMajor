using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Start with doors closed and then update the connected doors after room initialization

        // Delay the activation of doors to ensure room connections are confirmed
        StartCoroutine(ActivateDoorsAfterDelay());
    }

    IEnumerator ActivateDoorsAfterDelay()
    {
        // Wait until the next frame to ensure room is fully initialized
        yield return null;

        // Now activate the connected doors
        ActivateConnectedDoors();
    }

    void Update()
    {
        if (name.Contains("End") && !updatedDoors)
        {
            updatedDoors = true;
        }
    }

    /*public void UpdateDoorStates()
    {
        foreach (Door door in doors)
        {
            if (door != null && door.gameObject != null)
            {
                bool isConnected = IsConnectedRoom(door.doorType);
                door.gameObject.SetActive(!isConnected); // Open doors if not connected
            }
        }
    }*/

    public void RemoveConnectedDoorsIfNoEnemies()
    {
        bool hasEnemies = false;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy"))
            {
                hasEnemies = true;
                break;
            }
        }

        if (hasEnemies)
        {
            foreach (Door door in doors)
            {
                if (door != null && door.gameObject != null)
                {
                    door.gameObject.SetActive(true); // Close all doors if there are enemies
                }
            }
        }
        else
        {
            foreach (Door door in doors)
            {
                if (door != null && door.gameObject != null)
                {
                    door.gameObject.SetActive(false); // Open all doors if there are no enemies
                }
            }
        }
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
            isPlayerInRoom = true;
            RoomController.instance.OnPlayerEnterRoom(this);
            CheckForEnemies();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = false;
            CheckForEnemies();
        }
    }

    public void CheckForEnemies()
    {
        StartCoroutine(CheckForEnemiesFix());
    }

    private IEnumerator CheckForEnemiesFix()
    {
        Debug.Log("CHECKFIREBEMIES");
        yield return new WaitForSeconds(.3f);
        if (isPlayerInRoom)
        {
                bool hasEnemies = false;

            foreach (Transform child in transform)
            {
                if (child.CompareTag("Enemy"))
                {
                    hasEnemies = true;
                    break;
                }
            }

            if (hasEnemies)
            {
                Debug.Log("has enemies");
                foreach (Door door in doors)
                {
                    if (door != null && door.gameObject != null)
                    {
                        door.gameObject.SetActive(true); // Close doors if there are enemies
                    }
                }
            }
            else
            {
                Debug.Log("no enemies");
                ActivateConnectedDoors();
            }
        }
    }
    public void CheckForEnemiesWithDelay()
    {
        StartCoroutine(CheckForEnemiesWithDelayCoroutine());
    }

    private IEnumerator CheckForEnemiesWithDelayCoroutine()
    {
        yield return new WaitForSeconds(3f);
        CheckForEnemies();
        yield return new WaitForSeconds(1f);
        CheckForEnemies();
    }

    public void ActivateConnectedDoors()
    {
        Debug.Log("activatingConnectedDoors");
        foreach (Door door in doors)
        {
            if (door != null && door.gameObject != null)
            {
                // Only open doors with a connected room
                if (IsConnectedRoom(door.doorType))
                {
                    door.gameObject.SetActive(false); // Open doors if connected
                }
                else
                {
                    door.gameObject.SetActive(true); // Keep doors closed if not connected
                }
            }
        }
    }
}
