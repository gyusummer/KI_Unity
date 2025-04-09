using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeMakeAndLoad : MonoBehaviour
{
    public GameObject CubePrefab;

    private void Start()
    {
        LoadCube();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.collider is not null)
            {
                Vector3 rRot = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                Instantiate(CubePrefab, hit.point, Quaternion.Euler(rRot));
            }
        }
    }

    private void LoadCube()
    {
        for (int i = 0; i < SaveDataContainer.Instance.LoadData.Count; i++)
        {
            string[] tokens = SaveDataContainer.Instance.LoadData[i];
            if (tokens[0] == "Cube")
            {
                Vector3 position = new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                Vector3 euler = new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6]));
                Instantiate(CubePrefab, position, Quaternion.Euler(euler));
            }
        }
    }
}
