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
    private float aliveTime;
    
    private RectTransform rectTransform;
    public bool IsInitialized = false;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (aliveTime > duration)
        {
            ReturnToPool();
            return;
        }
        aliveTime += Time.deltaTime;
        
        Vector3 screenPos = camera.WorldToScreenPoint(target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, camera, out Vector2 localPos);
        rectTransform.anchoredPosition = localPos;

        rectTransform.anchoredPosition += Vector2.up * (aliveTime * 200f);
    }

    public void Initialize(Camera camera, Canvas canvas)
    {
        this.camera = camera;
        this.canvas = canvas;
    }
    
    public void Set(Transform target, string content, Color color, float duration)
    {
        aliveTime = 0;
        text.color = color;
        this.target = target;
        text.text = content;
        this.duration = duration;
    }
    
    public void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(this);
    }
}
