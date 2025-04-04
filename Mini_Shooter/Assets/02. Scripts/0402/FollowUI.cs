using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowUI : MonoBehaviour, IObjectPoolItem
{
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    private Camera camera;
    private Canvas canvas;
    
    private Transform target;
    private TMP_Text text;
    private float duration;
    
    private RectTransform rectTransform;
    public bool IsInitialized = false;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (duration < 0)
        {
            ReturnToPool();
            return;
        }
        duration -= Time.deltaTime;

        Vector3 viewPos = camera.WorldToViewportPoint(target.position);
        if (viewPos.z <= 0)
        {
            return;
        }
        
        Vector3 screenPos = camera.WorldToScreenPoint(target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, camera, out Vector2 localPos);
        rectTransform.anchoredPosition = localPos;
    }

    public void Initialize(Camera camera, Canvas canvas)
    {
        this.camera = camera;
        this.canvas = canvas;
    }
    
    public void Set(Transform worldTarget, string content, Color color, float duration)
    {
        text.color = color;
        this.target = worldTarget;
        text.text = content;
        this.duration = duration;
    }
    
    public void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(this);
    }
}
