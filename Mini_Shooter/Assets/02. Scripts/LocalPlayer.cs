using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player, IDamageAble
{
    public Collider MainCollider { get; }
    public GameObject GameObject { get; }

    private void Awake()
    {
        Player.LocalPlayer = this;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        
    }
}
