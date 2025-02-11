using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject guitubble;
    public Slider healthSlider;
    public float defense;

    void Start()
    {
        guitubble.SetActive(false);
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
    IEnumerator defenseReset()
    {
        yield return new WaitForSeconds(3f);
        defense = 0;
        guitubble.SetActive(false);
    }
}
