using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    public static BlockSystem Instance { get; private set; }
    
    [Header("Prefabs")]
    public GameObject[] blockPrefab;

    private int cursor = -1;
    
    private GameObject previewObject;
    private Vector3Int lastPreviewGridPos;
    private List<BlockData> placedBlocks = new List<BlockData>();
    private GridSystem gridSystem;
    
    private Quaternion currentRotation = Quaternion.identity;
    
    private void Awake()
    {
        Instance = this;
        gridSystem = new GridSystem(Vector3.one);
    }

    private void Start()
    {
        ChangeCursor(0);
        LoadBlocksFromCsv();
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeCursor(cursor-1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeCursor(cursor+1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SaveBlocksToCsv();
        }
      
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation *= Quaternion.Euler(0, 90, 0);
            previewObject.transform.rotation = currentRotation;
        }
        
        UpdatePreviewBlock();
    }

    private void ChangeCursor(int index)
    {
        int prev = cursor;
        cursor = index;
        cursor = Mathf.Clamp(cursor, 0, blockPrefab.Length - 1);
        if (prev != cursor) CreatePreviewBlock();
    }

    private void CreatePreviewBlock()
    {
        if(previewObject != null) Destroy(previewObject);
        previewObject = Instantiate(blockPrefab[cursor]);
        previewObject.GetComponentInChildren<Collider>().enabled = false;
    }
    
    private void UpdatePreviewBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3Int gridPos = gridSystem.WorldToGrid(hit.point);

            if (gridPos != lastPreviewGridPos)
            {
                lastPreviewGridPos = gridPos;
                Vector3 worldPos = gridSystem.GridToWorld(gridPos);
                previewObject.transform.position = worldPos;
                previewObject.transform.rotation = currentRotation;
                previewObject.SetActive(true);

                // 이미 블록이 존재하면 색상을 빨강으로 바꿈
                if (IsBlocked())
                    SetPreviewColor(Color.red);
                else
                    SetPreviewColor(new Color(0f, 1f, 0f, 0.4f)); // 초록 반투명
            }
        }
        else
        {
            previewObject.SetActive(false);
        }
    }

    private void SetPreviewColor(Color color)
    {
        var renderer = previewObject.GetComponentInChildren<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            renderer.material.color = color;
        }
    }

    
    public bool PlaceBlock(Vector3 position)
    {
        Vector3Int gridPos = gridSystem.WorldToGrid(position);
        
        if (IsBlocked())
        {
            Debug.Log($"Place Block : false, {position}");
            return false;
        }

        GameObject block = Instantiate(blockPrefab[cursor], gridPos, currentRotation);
        BlockData data = new BlockData(block, cursor, gridPos, currentRotation.y);
        placedBlocks.Add(data);

        Debug.Log($"Place Block : true, {gridPos}, {position}");
        return true;
    }
    
    public bool RemoveBlock(Vector3 position)
    {
        for (int i = 0; i < placedBlocks.Count; i++)
        {
            if (placedBlocks[i].Bounds.Contains(position))
            {
                Destroy(placedBlocks[i].instance);
                placedBlocks.RemoveAt(i);
                Debug.Log($"Remove Block : true, {position}");
                return true;
            }
        }

        Debug.Log($"Remove Block : false, {position}");
        return false;
    }

    private bool IsBlocked()
    {
        if (previewObject == null) return true;

        Bounds previewBounds = BoundsUtil.CalculateRendererBounds(previewObject);

        foreach (var block in placedBlocks)
        {
            if (block.Overlaps(previewBounds))
            {
                return true;
            }
        }

        return false;
    }
    
    private void SaveBlocksToCsv()
    {
        string path = Path.Combine(Application.persistentDataPath, "blocks.csv");
        using (var sw = new StreamWriter(path))
        using (var cw = new CsvWriter(sw, CultureInfo.CurrentCulture))
        {
            List<BlockDataCSV> csvDatas = new List<BlockDataCSV>();
            foreach (var block in placedBlocks)
            {
                csvDatas.Add(block.Csv);
            }
            cw.WriteRecords(csvDatas);
        }
        
        Debug.Log($"Saved to {path}");
    }
    
    private void LoadBlocksFromCsv()
    {
        string path = Path.Combine(Application.persistentDataPath, "blocks.csv");

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        using (CsvReader cr = new CsvReader(new StreamReader(path), CultureInfo.CurrentCulture))
        {
            var blockDatas = cr.GetRecords<BlockDataCSV>();
            placedBlocks = new List<BlockData>();

            foreach (var csvData in blockDatas)
            {
                Vector3 position = csvData.GetPosition();
                Quaternion rotation = Quaternion.Euler(0, csvData.YRotation, 0);
                GameObject block = Instantiate(blockPrefab[csvData.PrefabIndex], position, rotation);
                BlockData data = new BlockData(block, csvData);
                placedBlocks.Add(data);
            }
        }

        Debug.Log($"Loaded from {path}");
    }
}
