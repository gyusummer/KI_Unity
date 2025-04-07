using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour, IObjectPoolItem
{
    public string Key { get; set; }
    public GameObject GameObject => gameObject;

    public ParticleSystem particleSystem;

    private void Update()
    {
        if (particleSystem.isPlaying == false)
        {
            ReturnToPool();
            gameObject.SetActive(false);
        }
    }

    public void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(this);
    }
}
