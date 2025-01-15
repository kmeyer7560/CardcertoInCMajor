using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollStop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RollStop()
    {
        Debug.Log("rollstop");
        gameObject.SetActive(false);
    }
}
