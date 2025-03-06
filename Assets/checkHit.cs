using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkHit : MonoBehaviour
{
    public bool isHit;
    void TriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isHit = true;
        }
        else
        {
            isHit = false;
        }
    }
}
