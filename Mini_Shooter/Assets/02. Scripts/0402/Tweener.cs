using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour, IObjectPoolItem
{
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    
    private float duration;

    private void Update()
    {
        if (duration < 0)
        {
            ReturnToPool();
            return;
        }
        duration -= Time.deltaTime;
        transform.Translate(Vector3.up * Time.deltaTime, Space.World);
    }
    public void Set(float duration)
    {
        this.duration = duration;
    }

    public void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(this);
    }
}
