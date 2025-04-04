using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Breath : MonoBehaviour
{
    public Collider Collider;
    
    private List<Collider> colliders = new List<Collider>();
    private Transform breathPoint; //위치 벡터 참조 트랜스 폼
    private Transform breathDir; // 방향벡터 참조 트랜스폼
    
    public void SetProperty(Transform worldPoint, Transform worldDir)
    {
        breathPoint = worldPoint;
        breathDir = worldDir;
    }

    public void ResetColliders()
    {
        colliders.Clear();
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = breathPoint.position;
        transform.forward =  breathDir.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(colliders.Contains(other)) return;

        if (other.Equals(Player.LocalPlayer.MainCollider))
        {
            CombatEvent combatEvent = new CombatEvent();
                
            combatEvent.Sender = BossMonster.CrrentSceneBossMonster;
            combatEvent.Receiver = Player.LocalPlayer;
            combatEvent.Damage = 1;
            combatEvent.HitPosition = Player.LocalPlayer.transform.position;
            combatEvent.Collider = Player.LocalPlayer.MainCollider;
                
            CombatSystem.Instance.AddCombatEvent(combatEvent);
        }
        
        // var capsuleWarrior = CapsuleWarrior.GetCapsuleWarrior(other);
        // if (capsuleWarrior != null)
        // {
        //     capsuleWarrior.ChangeHp(-1);
        //     colliders.Add(other);
        // }
    }
}
