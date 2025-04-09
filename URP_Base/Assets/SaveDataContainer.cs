using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataContainer : MonoBehaviour
{
    public static SaveDataContainer Instance;
    public List<String[]> LoadData = new List<String[]>();
    
    public List<ISaveObject> SaveObjects = new List<ISaveObject>();
    private string savePath;

    private void Awake()
    {
        Instance = this;
        savePath = Application.persistentDataPath + "/CubeSaveAndLoad.txt";
        
        LoadSaveData();
    }

    private IEnumerator Start()
    {
        yield return null;
        LoadData.Clear();
        GC.Collect();
    }

    private void OnApplicationQuit()
    {
        StreamWriter writer = new StreamWriter(savePath);
        for (int i = 0; i < SaveObjects.Count; i++)
        {
            writer.WriteLine(SaveObjects[i].SaveData);
        }
        writer.Close();
    }

    private void LoadSaveData()
    {
        StreamReader reader = new StreamReader(savePath);
        while (reader.EndOfStream == false)
        {
            string line = reader.ReadLine();
            string[] tokens = line.Split(',');
            LoadData.Add(tokens);
        }
        reader.Close();
    }
}
