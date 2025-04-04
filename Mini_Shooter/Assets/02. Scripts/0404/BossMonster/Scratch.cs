using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    public Collider Collider;
    
    private void OnTriggerEnter(Collider other)
    {
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
        // }
    }
}
