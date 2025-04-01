using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFXDmg : MonoBehaviour
{
    private bool cooldown = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(cooldown)
            {
                cooldown = false;
                other.GetComponent<EnemyHealth>().takeDamage(5);
                StartCoroutine(CooldownTimer());
            }
        }
    }
    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(.5f);
        cooldown = true;
    }
}
