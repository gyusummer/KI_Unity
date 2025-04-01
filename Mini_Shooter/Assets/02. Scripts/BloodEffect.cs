using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(ParticleSystem))]
public class BloodEffect : MonoBehaviour
{
    public ParticleSystem system;
    public IObjectPool<ParticleSystem> pool;

    void Start()
    {
        system = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = system.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        // Return to the pool
        pool.Release(system);
    }
}