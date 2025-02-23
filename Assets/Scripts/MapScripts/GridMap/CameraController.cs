using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;
    public Room currRoom;
    public float moveSpeedWhenRoomChange;
    private Room currentRoom;

    void Awake()
    {
        instance = this;
        currentRoom = GetComponentInParent<Room>();
    }
    
    void Update()
    {
        UpdatePosition();
        //if(instance.currRoom == currentRoom)
        //{
        //    //currentRoom.SetActive(true);
        //}
    }

    void UpdatePosition()
    {
        if(currRoom == null)
        {
            return;
        }
        Vector3 targetPos = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    Vector3 GetCameraTargetPosition()
    {
        if(currRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currRoom.GetRoomCenter();
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
