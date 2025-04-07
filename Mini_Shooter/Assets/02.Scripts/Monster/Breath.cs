using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Breath : MonoBehaviour
{
    public Collider Collider;
    
    private List<Collider> colliders = new List<Collider>();
    private Transform breathPoint; //위치 벡터 참조 트랜스 폼
    private Transform breathDir; // 방향벡터 참조 트랜스폼
    
    public void SetProperty(Transform worldPoint, Transform worldDir)
    {
        breathPoint = worldPoint;
        breathDir = worldDir;
    }

    public void ResetColliders()
    {
        colliders.Clear();
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = breathPoint.position;
        transform.forward =  breathDir.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
