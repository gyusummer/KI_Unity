using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour, IDamageAble
{
    public Collider MainCollider => spineCollider;
    public GameObject GameObject => gameObject;

    public Collider spineCollider;

    private void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        
    }
}
