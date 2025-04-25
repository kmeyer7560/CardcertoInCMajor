using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject guitubble;
    public GameObject vioubble;
    public GameObject drubble;
    public Slider healthSlider;
    public float defense;
    private bool deflectActive;
    public int deflectedNum;
    public GameObject player;

    void Start()
    {
        vioubble.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public void SetSlider(float amount)
    {
        healthSlider.value = amount;
    }

    public void SetSliderMax(float amount)
    {
        healthSlider.maxValue = amount;
        SetSlider(amount);
    }

    public void TakeDamage(float amount)
    {
        if (deflectActive)
        {
            amount = 0;
            deflectedNum++; 
        }
        
        healthSlider.value -= (amount - defense);
    }

    public void Heal(float amount, int i)
    {
        Debug.Log("player healed");
        healthSlider.value += (amount * i);
    }

    public void setDefense(float value, int i)
    {
        if (i == 1)
        {
            GameObject instantiateGuitubble = Instantiate(guitubble, new Vector3(player.transform.position.x-.3f, player.transform.position.y, player.transform.position.z), transform.rotation);
            instantiateGuitubble.transform.SetParent(player.transform);
        }
        else if (i == 2)
        {
            drubble.SetActive(true);
        }
        defense = value;
        StartCoroutine(defenseReset());
    }

    public void deflect(Action<int> callback)
    {
        vioubble.SetActive(true);
        deflectActive = true;
        StartCoroutine(reflectRoutine(callback));
    }

    IEnumerator defenseReset()
    {
        yield return new WaitForSeconds(1f);
        defense = 0;
        drubble.SetActive(false);
    }

    IEnumerator reflectRoutine(Action<int> callback)
    {
        yield return new WaitForSeconds(1f);
        deflectActive = false;
        vioubble.SetActive(false);
        callback(deflectedNum); // Call the callback with the deflectedNum
    }
}
