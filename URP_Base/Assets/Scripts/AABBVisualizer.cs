using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBVisualizer : MonoBehaviour
{
    public Color boundColor = Color.red;

    private MeshFilter meshFilter;

    public Vector3 rotateValue;
    public float speed;
    
    private void Update()
    {
        transform.Rotate(rotateValue * (speed * Time.deltaTime)); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = boundColor;

        if (meshFilter == null)
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null) return;
        }
            
        //로컬에 있음
        Bounds bounds = meshFilter.sharedMesh.bounds;
        
        //로컬 -> 월드 변환
        bounds = TransformBounds(bounds, transform.localToWorldMatrix);
        
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        
    }

    private Bounds TransformBounds(Bounds localBounds, Matrix4x4 worldMatrix)
    {
        Bounds reVal = default;

        Vector3 center = worldMatrix.MultiplyPoint(localBounds.center); 
        Vector3 extents = localBounds.extents;

        Vector3 axisX = worldMatrix.MultiplyVector(Vector3.right) * extents.x;
        Vector3 axisY = worldMatrix.MultiplyVector(Vector3.up) * extents.y;
        Vector3 axisZ = worldMatrix.MultiplyVector(Vector3.forward) * extents.z;

        Vector3 worldExtents = new Vector3(
            Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x), //끝이 x인 녀석들 다 더함
            Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y), //끝이 y인 녀석들 다 더함
            Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z) //끝이 z인 녀석들 다 더함
        );

        reVal = new Bounds(center, worldExtents * 2f);
        
        return reVal;
    }
}
