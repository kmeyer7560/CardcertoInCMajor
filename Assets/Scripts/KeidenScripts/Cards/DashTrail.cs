using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashTrail : MonoBehaviour
{

    public BoxCollider2D bc;
    public float damage;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(damage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(damage);
            StartCoroutine(laserCooldown());
            
        }
    }

    IEnumerator laserCooldown()
    {
        yield return new WaitForSeconds(0.3f);
    }
}
