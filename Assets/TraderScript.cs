using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderScript : MonoBehaviour
{
    private bool canMove;
    public GameObject shopMenu;
    private bool playerInRoom;
    [SerializeField] float speed = .5f;
    public Room currentRoom;

    public GameObject player;
    [SerializeField] Transform playerTransform;
    private Animator animator;
    private bool shopOpen;



    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform.Find("Sprite");
        animator = GetComponent<Animator>();
        currentRoom = GetComponentInParent<Room>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerInRoom();
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 2f && Input.GetKeyDown(KeyCode.Space))
        {
            OpenShop();
        }
        if(distance<10f && distance>.5f && !shopOpen)
        {
            canMove= true;
        }
        else
        {
            canMove = false;
        }
        if(canMove)
        {
        }
    }
    void UpdatePlayerInRoom()
    {
        CameraController cameraController = CameraController.instance;
        if (cameraController != null)
        {
            playerInRoom = cameraController.currRoom == currentRoom;
        }
        else
        {
            playerInRoom = false;
        }
    }

    public void OpenShop()
    {
        //Debug.Log("OPENSHOP");
        shopOpen = true;
        canMove = false;
        shopMenu.SetActive(true);

    }
    
}
