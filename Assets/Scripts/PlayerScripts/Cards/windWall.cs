using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class windWall : MonoBehaviour
{
    public Rigidbody2D rb;
    

    public void activate(float angle, Vector2 pos)
    {
        StartCoroutine(Wall(angle, pos));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wall(float an, Vector2 position)
    {
        rb.position = position;
        yield return new WaitForSeconds(3f);
    }
}
