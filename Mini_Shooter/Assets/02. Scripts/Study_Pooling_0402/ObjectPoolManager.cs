using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPoolData
    {
        public string Key;
        public GameObject Prefab;
        public Transform Parent;
        public byte ExpandSize;
    }
    public static ObjectPoolManager Instance;
    
    public ObjectPoolData[] ObjectPoolDatas;
    
    private Dictionary<string, ObjectPool> objectPoolDict = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (ObjectPoolData poolData in ObjectPoolDatas)
        {
            CreatePool(poolData);
        }
    }

    public void CreatePool(ObjectPoolData data)
    {
        if (objectPoolDict.ContainsKey(data.Key))
        {
            Debug.LogWarning($"Pool with key \"{data.Key}\" already exists.");
            return;
        }
        ObjectPool pool = new ObjectPool();
        var sample = data.Prefab.GetComponent<IObjectPoolItem>();

        if (sample == null)
        {
            Debug.LogWarning($"{data.Prefab.name} does not implement IObjectPoolItem.");
        }
        
        pool.Initialize(sample, data.Parent, data.ExpandSize, data.Key);
        objectPoolDict.Add(data.Key, pool);
    }

    public IObjectPoolItem GetObjectPoolOrNull(string key)
    {
        if (objectPoolDict.ContainsKey(key) == false)
        {
            Debug.LogWarning($"Pool with key \"{key}\" does not exist.");
            return null;
        }
        return objectPoolDict[key].Get();
    }

    public void ReturnToPool(IObjectPoolItem item)
    {
        if (objectPoolDict.ContainsKey(item.Key) == false)
        {
            Debug.LogWarning($"Pool with key \"{item.Key}\" does not exist.");
            return;
        }
        objectPoolDict[item.Key].ReturnToPool(item);
    }
}
