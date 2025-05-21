using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class fluteSlash : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject wallPoint;
    public GameObject fov;
    public Animator anim;
    public bool isSlash;


    public void activate()
    {
        StartCoroutine(Slash());
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(1000000, 100000);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSlash)
        {
            transform.position = wallPoint.transform.position;
        }
    }

    IEnumerator Slash()
    {
        rb.rotation = fov.GetComponent<FOV>().lastAngle;
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(55);
            other.GetComponent<EnemyHealth>().knockBack(this.gameObject);

        }
    }
}
