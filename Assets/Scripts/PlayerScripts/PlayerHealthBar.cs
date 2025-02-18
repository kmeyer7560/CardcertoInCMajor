using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject guitubble;
    public GameObject vioubble;
    public Slider healthSlider;
    public float defense;
    private bool deflectActive;
    public int deflectedNum;

    void Start()
    {
        guitubble.SetActive(false);
        vioubble.SetActive(false);
    }
    
    public void SetSlider (float amount)
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
        
        healthSlider.value -= (amount-defense);
    }
    public void Heal(float amount)
    {
        Debug.Log("player healed");
        healthSlider.value += (amount);
    }
    public void setDefense(float value)
    {
        guitubble.SetActive(true);
        defense = value;
        StartCoroutine(defenseReset());
    }

    public int deflect()
    {
        vioubble.SetActive(true);
        deflectActive = true;
        StartCoroutine(reflectRoutine());
        return deflectedNum;
    }
    IEnumerator defenseReset()
    {
        yield return new WaitForSeconds(3f);
        defense = 0;
        guitubble.SetActive(false);
    }
    IEnumerator reflectRoutine()
    {
        yield return new WaitForSeconds(1f);
        deflectActive = false;
        vioubble.SetActive(false);
    }
}
