using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject[] AllTiles;
    public GameObject ClosestTile;
    float distance;
    float nearestDistance;
    // Start is called before the first frame update
    void Start()
    {
        AllTiles = GameObject.FindGameObjectsWithTag("Floor");

        for(int i=0;i<AllTiles.Length;i++)
        {
            distance = Vector2.Distance(this.transform.position, AllTiles[i].transform.position);

            if(distance<nearestDistance)
            {
                ClosestTile = AllTiles[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
