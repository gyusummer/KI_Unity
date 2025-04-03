using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IObjectPoolItem
{
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private ParticleSystem explosion;

    private int damage;
    private void Update()
    {
        if (explosion.gameObject.activeInHierarchy && explosion.isPlaying == false)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (explosion.gameObject.activeInHierarchy) return;
        IDamageAble monster = CombatSystem.Instance.GetMonsterOrNull(other);
        if (monster is not null)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,1.0f, LayerMask.GetMask("Enemy"));
        Debug.Log(hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {
            CombatEvent combatEvent = new CombatEvent();
            
            combatEvent.Sender = Player.LocalPlayer;
            IDamageAble hit = CombatSystem.Instance.GetMonsterOrNull(hits[i]);
            combatEvent.Receiver = hit;
            combatEvent.Damage = damage;
            combatEvent.HitPosition = hits[i].ClosestPoint(transform.position);
            combatEvent.Collider = hits[i];
            
            CombatSystem.Instance.AddCombatEvent(combatEvent);
            
            explosion.gameObject.SetActive(true);
        }
    }
    public void LaunchGrenade(float force, int damage)
    {
        rigidBody.velocity = Vector3.zero;
        gameObject.SetActive(true);
        this.damage = damage;
        rigidBody.AddForce(transform.up * force, ForceMode.Impulse);
    }
    public void ReturnToPool()
    {
        explosion.gameObject.SetActive(false);
        ObjectPoolManager.Instance.ReturnToPool(this);
    }
}
