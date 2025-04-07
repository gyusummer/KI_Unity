using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowUI : MonoBehaviour, IObjectPoolItem
{
    // 텍스트 출력
    // 스스로 사용종료 처리를 하는것
    // 풀로 되돌아 가는것
    
    // 3차원 좌표계의 오브젝트(Transform)를 참조해서 실시간으로
    // 2차원 좌표계(Canva, Camera)로 위치좌표를 변환 하는 기능
    
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    
    private Camera camera;
    private Canvas canvas;

    private Transform target;
    private TMP_Text text;
    private float duration;

    private RectTransform rectTransform;
    
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
        
        camera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// 사용될때마다 달라지는 참조변수 및 데이터를 갱신하는 함수
    /// </summary>
    /// <param name="target"></param>
    /// <param name="content"></param>
    /// <param name="duration"></param>
    public void Set(Transform target, string content, float duration, Color color)
    {
        this.target = target;
        text.text = content;
        this.duration = duration;
        text.color = color;
    }

    private void Update()
    {
        //사용 종료 처리
        if (duration < 0)
        {
            ReturnToPool();
            return;
        }
        
        duration -= Time.deltaTime;

        Vector3 viewPort = camera.WorldToViewportPoint(target.position);
        bool isInViewPort = viewPort.z > 0;
        text.enabled = isInViewPort;
        
        //3차월 좌표계 -> 2차원 좌표계 전환.
        Vector3 screenPoint = camera.WorldToScreenPoint(target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (canvas.transform as RectTransform, screenPoint,
            camera, out Vector2 localPoint);

        rectTransform.anchoredPosition = localPoint;
    }


    public void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(this);
    }
}
