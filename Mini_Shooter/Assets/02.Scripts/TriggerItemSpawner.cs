using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TriggerItemSpawner : MonoBehaviour
{
    public TriggerItem[] TriggerItemPrefabs;
    public Transform[] spawnTransforms;

    public float spawnInterval = 3.0f;
    public float spawnRadius = 3.0f;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            var selectedItem = TriggerItemPrefabs[Random.Range(0, TriggerItemPrefabs.Length)];
            var selectedTransform = spawnTransforms[Random.Range(0, spawnTransforms.Length)];
            Vector2 circlePoint = Random.insideUnitCircle;
            
            Vector3 spawnPoint = selectedTransform.position;
            spawnPoint.x += Random.Range(-spawnRadius, spawnRadius) * circlePoint.x;
            spawnPoint.z += Random.Range(-spawnRadius, spawnRadius) * circlePoint.y;
            
            Instantiate(selectedItem, spawnPoint, selectedItem.transform.rotation);
        }
    }
}
