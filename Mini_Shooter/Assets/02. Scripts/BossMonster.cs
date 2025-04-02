using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour, IDamageAble
{
    public Collider MainCollider => spineCollider;
    public GameObject GameObject => gameObject;
    

    public Collider spineCollider;
    public Collider headCollider;
    public Collider[] leftArmCollider;
    public Collider[] rightArmCollider;

    private void Start()
    {
        CombatSystem.Instance.RegisterMonster(spineCollider,this);
        CombatSystem.Instance.RegisterMonster(headCollider,this);
        foreach (Collider collider in leftArmCollider)
        {
            CombatSystem.Instance.RegisterMonster(collider,this);
        }
        foreach (Collider collider in rightArmCollider)
        {
            CombatSystem.Instance.RegisterMonster(collider,this);
        }
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        Debug.Log($"{combatEvent.Collider.gameObject.name} damage");
    }

    public float HowPainful(Collider collider)
    {
        if (collider == headCollider)
        {
            return 2;
        }
        else if (collider == spineCollider)
        {
            return 1;
        }
        else
        {
            return 0.5f;
        }
    }
}
