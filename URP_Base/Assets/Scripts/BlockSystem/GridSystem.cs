using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    // 기본 셀 사이즈 (1,1,1)
    public Vector3 CellSize { get; private set; } = Vector3.one;

    public GridSystem(Vector3 cellSize)
    {
        this.CellSize = cellSize;
    }

    // 월드 좌표 → 그리드 좌표
    public Vector3Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector3Int(
            Mathf.FloorToInt(worldPos.x / CellSize.x),
            Mathf.FloorToInt(worldPos.y / CellSize.y),
            Mathf.FloorToInt(worldPos.z / CellSize.z)
        );
    }

    // 그리드 좌표 → 월드 좌표
    public Vector3 GridToWorld(Vector3Int gridPos)
    {
        return new Vector3(
            gridPos.x * CellSize.x,
            gridPos.y * CellSize.y,
            gridPos.z * CellSize.z
        );
    }
}

public class BlockData
{
    public GameObject instance;
    public Bounds Bounds;
    public BlockDataCSV Csv;

    public BlockData(GameObject instance, int prefabIndex, Vector3 position, float yRotation)
    {
        this.instance = instance;
        this.Bounds = BoundsUtil.CalculateRendererBounds(instance);
        Bounds.size *= 0.95f;
        Csv = new BlockDataCSV();
        Csv.PrefabIndex = prefabIndex;
        Csv.PosX = position.x;
        Csv.PosY = position.y;
        Csv.PosZ = position.z;
        Csv.YRotation = yRotation;
    }
    
    public BlockData(GameObject instance, BlockDataCSV csvData)
    {
        this.instance = instance;
        this.Bounds = BoundsUtil.CalculateRendererBounds(instance);
        Bounds.size *= 0.95f;
        Csv = new BlockDataCSV();
        Csv.PrefabIndex = csvData.PrefabIndex;
        Csv.PosX = csvData.PosX;
        Csv.PosY = csvData.PosY;
        Csv.PosZ = csvData.PosZ;
        Csv.YRotation = csvData.YRotation;
    }

    public bool Overlaps(Bounds other) => Bounds.Intersects(other);
}

public class BlockDataCSV
{
    public int PrefabIndex { get; set; }
    
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    
    public float YRotation { get; set; }
    
    public Vector3 GetPosition()
    {
        return new Vector3(PosX, PosY, PosZ);
    }
}

public static class BoundsUtil
{
    public static Bounds CalculateRendererBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }
}