using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{   
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Destroy(.5f));
        animator.SetTrigger("play");
    }

    IEnumerator Destroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
