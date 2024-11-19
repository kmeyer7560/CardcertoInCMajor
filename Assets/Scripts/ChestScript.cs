using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private string[] lootpool;
    public GameObject player;
    private int range = 3;
    private int randomDraw;

    public string gun1;
    public string gun2;
    public string gun3;
    private int reward;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");x
        //lootpool = [gun1, gun2, gun3];
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if(Input.GetKeyDown("space") && distance <= range)
        {
            OpenChest();
        }
    }
    void OpenChest()
    {
        //play chest open animatio
        randomDraw = Random.Range(0,2);
        //reward = lootpool[randomDraw];

    }
}
