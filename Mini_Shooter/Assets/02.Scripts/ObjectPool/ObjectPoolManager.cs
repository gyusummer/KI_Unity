using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    
    [System.Serializable]
    public class ObjectPoolData
    {
        public string Key;
        public GameObject Prefab;
        public byte ExpandSize;
        public Transform Parent;
    }
    
    public ObjectPoolData[] ObjectPoolDatas;
    
    private Dictionary<string, ObjectPool> objectPoolDic 
        = new Dictionary<string, ObjectPool>();
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var data in ObjectPoolDatas)
        {
            CreatePool(data);
        }
    }

    public void CreatePool(ObjectPoolData data)
    {
        if (objectPoolDic.ContainsKey(data.Key))
        {
            Debug.LogWarning($"{data.Key}값이 Pool에 이미 존재합니다.");
            return;
        }
        
        var pool = new ObjectPool();
        var poolItem = data.Prefab.GetComponent<IObjectPoolItem>();

        if (poolItem == null)
        {
            Debug.LogWarning($"{data.Prefab.name}의 프리펩에 IObjectPoolItem을" +
                             $"상속받은 컴포넌트가 없습니다.");
            return;
        }
        
        Transform parent = data.Parent == null ? transform : data.Parent.transform;
        pool.Initialize(poolItem, parent, data.ExpandSize, data.Key);
        objectPoolDic.Add(data.Key, pool);
    }

    public IObjectPoolItem GetObjectOrNull(string key)
    {
        if (objectPoolDic.ContainsKey(key) == false)
        {
            Debug.LogWarning($"{key}값의 Pool이 존재하지 않습니다");
            return null;
        }
        return objectPoolDic[key].Get();
    }

    public void ReturnToPool(IObjectPoolItem item)
    {
        if (objectPoolDic.ContainsKey(item.Key) == false)
        {
            Debug.LogWarning($"{item.Key}값의 Pool이 존재하지 않습니다");
            return;
        }
        objectPoolDic[item.Key].Return(item);
    }
    
}
