using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleEffects : MonoBehaviour
{
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!ps.isPlaying)
        {
            Destroy(gameObject);
        }
    }

}


