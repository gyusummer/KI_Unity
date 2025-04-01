using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TestEffects : MonoBehaviour
{
    public GameObject TestEffectPrefab;
    
    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 100;

    private IObjectPool<ParticleSystem> m_Pool;
    public IObjectPool<ParticleSystem> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            }
            return m_Pool;
        }
    }
    private void Start()
    {
        CombatSystem.Instance.Events.OnCombatEvent += PlayBlood;
    }
    private void OnDestroy()
    {
        CombatSystem.Instance.Events.OnCombatEvent -= PlayBlood;
    }
    private ParticleSystem CreatePooledItem()
    {
        GameObject go = Instantiate(TestEffectPrefab);
        BloodEffect bloodEffect = go.AddComponent<BloodEffect>();
        bloodEffect.pool = Pool;

        ParticleSystem system = bloodEffect.system;

        return system;
    }
    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(ParticleSystem system)
    {
        system.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject(ParticleSystem system)
    {
        Destroy(system.gameObject);
    }
    
    private void PlayBlood(CombatEvent combatEvent)
    {
        ParticleSystem system = Pool.Get();
        system.transform.position = combatEvent.HitPosition;
    }
}