using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestEffects : MonoBehaviour
{
    public Camera Camera;
    public Canvas Canvas;
    private void Start()
    {
        Camera = Camera.main;
        
        CombatSystem.Instance.Events.OnCombatEvent += PlayBlood;
        CombatSystem.Instance.Events.OnCombatEvent += PrintDamageText;
    }
    private void OnDestroy()
    {
        CombatSystem.Instance.Events.OnCombatEvent -= PlayBlood;
        CombatSystem.Instance.Events.OnCombatEvent -= PrintDamageText;
    }
    private void PlayBlood(CombatEvent combatEvent)
    {
        var blood = ObjectPoolManager.Instance.GetObjectPoolOrNull("Blood");
        blood.GameObject.transform.position = combatEvent.HitPosition;
        blood.GameObject.SetActive(true);
    }
    private void PrintDamageText(CombatEvent combatEvent)
    {
        Transform worldTarget = new GameObject().transform;
        worldTarget.position = combatEvent.HitPosition;
        
        Color color = Color.white;
        float howPainful = combatEvent.Receiver.HowPainful(combatEvent.Collider);
        switch (howPainful)
        {
            case 0.5f:
                color = Color.white;
                break;
            case 1:
                color = Color.yellow;
                break;
            case 2:
                color = Color.red;
                break;
        }
        ShowText($"{combatEvent.Damage}", worldTarget, color);
    }

    private void ShowText(string message, Transform worldTarget, Color color)
    {
        var poolItem = ObjectPoolManager.Instance.GetObjectPoolOrNull("DamageText");
        var followUI = poolItem as FollowUI;
        if (followUI.IsInitialized == false)
        {
            followUI.Initialize(Camera, Canvas);
        }
        
        var gameObj = followUI.gameObject;
        gameObj.SetActive(true);
        followUI.Set(worldTarget, message, color, 3.0f);
    }
}