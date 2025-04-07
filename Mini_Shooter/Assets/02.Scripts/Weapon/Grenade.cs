using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Collider collider;
    
    public int Damage { get; set; }
    
    private void Update()
    {
        // explosion이 활성화 되고, explosion의 파티클이 재생되지 않는 상황이라면
        if (explosion.gameObject.activeInHierarchy && explosion.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var monster = CombatSystem.Instance.GetMonsterOrNull(other);
        if (monster != null)
        {
            explosion.gameObject.SetActive(true);
            
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Sender = Player.LocalPlayer;
            combatEvent.Receiver = monster;
            combatEvent.Damage = Damage;
            combatEvent.HitPosition = transform.position;
            combatEvent.Collider = other;

            CombatSystem.Instance.AddInGameEvent(combatEvent);

            collider.enabled = false;
        }
    }

    public void LaunchGrenade(float force)
    {
        gameObject.SetActive(true);
        rigidBody.AddForce(-transform.up * force, ForceMode.Force);
    }
}
