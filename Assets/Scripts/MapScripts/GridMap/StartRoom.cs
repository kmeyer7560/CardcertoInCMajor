using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoomDoorController : MonoBehaviour
{
    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    private List<Door> doors = new List<Door>();
    private Room room;  // Add a reference to the Room component.

    void Start()
    {
        // Get the Room component attached to the same GameObject
        room = GetComponent<Room>();

        if (room == null)
        {
            Debug.LogError("StartRoomDoorController: Room component not found on the Start Room.");
            return;
        }

        // Add the doors to a list for easy access
        doors.Add(leftDoor);
        doors.Add(rightDoor);
        doors.Add(topDoor);
        doors.Add(bottomDoor);

        // Start the process of checking for connected rooms after a delay
        StartCoroutine(CheckForConnectedRoomsAndOpenDoors());
    }

    // Coroutine to wait for 1 second, then check for connected rooms
    IEnumerator CheckForConnectedRoomsAndOpenDoors()
    {
        // Wait for 1 second to give time for initialization
        yield return new WaitForSeconds(1f);

        // Check for connected rooms, if none, open the corresponding doors
        foreach (Door door in doors)
        {
            if (door != null && door.gameObject != null)
            {
                bool isConnected = IsConnectedRoom(door.doorType);
                if (!isConnected)
                {
                    // If no connected room, open the door (set active)
                    door.gameObject.SetActive(true);
                }
                else
                {
                    // If there is a connected room, keep the door closed
                    door.gameObject.SetActive(false);
                }
            }
        }
    }

    // Simple check for connected rooms based on door type
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

    // Get the adjacent room to the right (this can be extended for other directions)
    private Room GetRight()
    {
        if (RoomController.instance.DoesRoomExist(room.X + 1, room.Y))
            return RoomController.instance.FindRoom(room.X + 1, room.Y);
        return null;
    }

    private Room GetLeft()
    {
        if (RoomController.instance.DoesRoomExist(room.X - 1, room.Y))
            return RoomController.instance.FindRoom(room.X - 1, room.Y);
        return null;
    }

    private Room GetTop()
    {
        if (RoomController.instance.DoesRoomExist(room.X, room.Y + 1))
            return RoomController.instance.FindRoom(room.X, room.Y + 1);
        return null;
    }

    private Room GetBottom()
    {
        if (RoomController.instance.DoesRoomExist(room.X, room.Y - 1))
            return RoomController.instance.FindRoom(room.X, room.Y - 1);
        return null;
    }
}
