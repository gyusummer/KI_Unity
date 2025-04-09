using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveObject
{
    public string SaveData { get; }
    public void SetSaveData();
}
