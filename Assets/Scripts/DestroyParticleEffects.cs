using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleEffects : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!particleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
    }

}


