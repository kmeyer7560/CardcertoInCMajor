using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    void Start()
    {
        guitubble.SetActive(false);
        vioubble.SetActive(false);
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
            guitubble.SetActive(true);
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
        yield return new WaitForSeconds(3f);
        defense = 0;
        guitubble.SetActive(false);
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
