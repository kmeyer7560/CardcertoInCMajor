using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class Guitubble : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleSystem;
    public GameObject player;
    void Start()
    {
        particleSystem.Play();
    }
}
