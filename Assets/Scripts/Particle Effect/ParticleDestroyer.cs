using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    void OnEnable()
    {
        // Ensure we have the latest particle systems
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (var ps in particleSystems)
        {
            ps.Stop();   // Stop it first to ensure it's reset
            ps.Clear();  // Clear the current particles
            ps.Play();   // Start the particle system
        }
    }

    void Update()
    {
        bool allFinished = true;

        foreach (var ps in particleSystems)
        {
            // Debug.Log($"Particle system {ps.name} isAlive: {ps.IsAlive()}");
            if (ps != null && ps.IsAlive())
            {
                allFinished = false;
                break;
            }
        }

        // Deactivate the game object when all particle systems have finished
        if (allFinished)
        {
            ObjectPooler.Instance.ReturnToPool(gameObject);  // New method to return to pool.
        }
    }
}

