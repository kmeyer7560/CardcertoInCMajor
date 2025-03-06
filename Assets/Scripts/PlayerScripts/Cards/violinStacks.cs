using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class violinStacks : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Vector3 speed;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        SetMaxParticles(0);
    }

    public void SetMaxParticles(int maxParticles)
    {
        var main = particleSystem.main;
        main.maxParticles = maxParticles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
