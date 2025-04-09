using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, ISaveObject
{
    private string saveData;

    public string SaveData => saveData;

    private void Start()
    {
        SetSaveData();
    }

    public void SetSaveData()
    {
        saveData =
            $"Cube,{transform.position.x},{transform.position.y},{transform.position.z},{transform.eulerAngles.x},{transform.eulerAngles.y},{transform.eulerAngles.z}";
        if (SaveDataContainer.Instance.SaveObjects.Contains(this) == false)
        {
            SaveDataContainer.Instance.SaveObjects.Add(this);
        }
    }

    private void OnDestroy()
    {
        SaveDataContainer.Instance.SaveObjects.Remove(this);
    }
}
