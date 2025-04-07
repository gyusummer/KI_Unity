using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssultRifle : Weapon
{
    public Transform Ejector;
    
    private Camera mainCam;
    
    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash.SetActive(false);
        mainCam = Camera.main;
    }

    public override bool Fire()
    {
        bool isAbleFire = IsAbleFire();
        if (isAbleFire == false) return false;
        
        muzzleFlash.transform.localRotation *= 
            Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
        
        muzzleFlash.SetActive(true);
        
        var shell = ObjectPoolManager.Instance.GetObjectOrNull("Shell");
        shell.GameObject.transform.position = Ejector.position;
        shell.GameObject.transform.right = Ejector.right;
        shell.GameObject.SetActive(true);
        
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 
                data.range, LayerMask.GetMask("Enemy")))
        {
            var monster = CombatSystem.Instance.GetMonsterOrNull(hit.collider);

            if (monster != null)
            {
                CombatEvent combatEvent = new CombatEvent();
                combatEvent.Sender = Player.LocalPlayer;
                combatEvent.Receiver = monster;
                combatEvent.Damage = data.damage;
                combatEvent.HitPosition = hit.point;
                combatEvent.Collider = hit.collider;
                
                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }

        return true;
    }
}
