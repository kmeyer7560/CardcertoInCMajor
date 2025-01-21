using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward : MonoBehaviour
{
    public string Value;
    public int Weight;
    public Sprite[] SuitSprites; // Array of 4 sprites for each suit
}
