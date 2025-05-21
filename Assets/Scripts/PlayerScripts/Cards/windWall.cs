using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class windWall : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject wallPoint;
    public GameObject fov;
    

    public void activate()
    {
        StartCoroutine(Wall());
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(1000000, 100000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wall()
    {
        Debug.Log("windwall up");
        rb.position = wallPoint.transform.position;
        rb.rotation = fov.GetComponent<FOV>().lastAngle;
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
