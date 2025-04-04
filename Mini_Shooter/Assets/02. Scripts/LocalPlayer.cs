using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player, IDamageAble
{
    public event Action OnHpChanged;
    public Collider MainCollider { get; private set; }
    public GameObject GameObject => gameObject;
    public Stat Stat => playerStat;
    private Stat playerStat = new Stat() { MaxHp = 100, CurrentHp = -1 };

    private void Awake()
    {
        Player.LocalPlayer = this;
        playerStat.CurrentHp = playerStat.MaxHp;
        MainCollider = GetComponent<Collider>();
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        Stat.CurrentHp -= combatEvent.Damage;
        OnHpChanged?.Invoke();
        Debug.Log("LocalPlayer takeDamage");
    }

    public float HowPainful(Collider collider)
    {
        return 0;
    }
}
