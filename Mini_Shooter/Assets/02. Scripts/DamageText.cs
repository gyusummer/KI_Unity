using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class DamageText : MonoBehaviour
{
    public TMP_Text text;
    public IObjectPool<DamageText> pool;
    private RectTransform canvas;
    private Camera mainCamera;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        mainCamera = Camera.main;
        canvas = transform.parent.GetComponent<RectTransform>();
    }
    
    public void Initialize(Vector3 position, int damage)
    {
        text.text = damage.ToString();
        text.color = Color.red; // Reset color to white
        
        Vector2 v2 = mainCamera.WorldToScreenPoint(position);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, v2, mainCamera, out Vector3 v3);
        transform.localPosition = v3;
        // transform.position = position;
    }

    private void Update()
    {
        // Move the text upwards
        transform.position += Vector3.up * Time.deltaTime;
        // Fade out the text
        Color color = text.color;
        color.a -= Time.deltaTime;
        text.color = color;

        // If the text is fully transparent, release it back to the pool
        if (color.a <= 0)
        {
            pool.Release(this);
        }
    }
}
