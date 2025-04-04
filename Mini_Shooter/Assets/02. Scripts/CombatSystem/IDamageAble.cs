using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamageAble
{
    public event Action OnHpChanged;
    public Collider MainCollider { get; }
    public GameObject GameObject { get; }
    public Stat Stat { get; }
    
    public void TakeDamage(CombatEvent combatEvent);
    public float HowPainful(Collider collider);
}
public class Stat
{
    // public const int HIT_COUNT = 20;
        
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    // public int CurrentHitCount { get; set; }
}
