using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    private ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particle != null && !particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
