using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour, IObjectPoolItem
{
    public string Key { get; set; }
    public GameObject GameObject => gameObject;

    private float yLength;
    private float duration;

    private void Update()
    {
        //사용 종료 처리
        if (duration < 0)
        {
            ReturnToPool();
            return;
        }
        
        duration -= Time.deltaTime;
        transform.Translate(Vector3.up * yLength * Time.deltaTime, Space.World);
    }

    public void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(this);
    }

    public void Set(float duration, float yLength = 2.0f)
    {
        this.duration = duration;
        this.yLength = yLength;
    }
}
