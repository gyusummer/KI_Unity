using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUpTable<TKey, TValue>
{
    private TKey[] keys;
    private TValue[] values;

    public LookUpTable(TKey[] keys, TValue[] values)
    {
        this.keys = keys;
        this.values = values;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Equals(key))
            {
                value = values[i];
                return true;
            }
        }

        value = default;
        return false;
    }
}