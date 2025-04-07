using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShotGun : Weapon
{
    [Header("Shotgun Settings")]
    public Transform fireTransform;

    public float distance = 0.5f;
    public float radius = 0.5f;
    
    public int bulletCount = 12;

    private void Start()
    {
        muzzleFlash.SetActive(false);
        CurrentAmmo = data.totalAmmo;
    }
    
    public override bool Fire()
    {
        bool isAbleFire = IsAbleFire();
        if (isAbleFire == false) return false;

        muzzleFlash.transform.localRotation *= 
            Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
        
        muzzleFlash.SetActive(true);

        //fireTransform의 위치에서 forwad방향으로 distance만큼 떨어진 점이 됨
        for (int i = 0; i < bulletCount; i++)
        {
            //원점 계산 및 변환
            Vector3 firePoint = fireTransform.TransformPoint(Vector3.zero);
            
            //목표점 계산 및 변환
            Vector2 circlePoint = Random.insideUnitCircle;
            circlePoint *= radius;
            
            Vector3 spreadPoint = 
                new Vector3(circlePoint.x, circlePoint.y, distance);
            
            spreadPoint = fireTransform.TransformPoint(spreadPoint);
            
            Ray ray = new Ray(firePoint, (spreadPoint - firePoint).normalized);
            Debug.DrawRay(firePoint, (spreadPoint - firePoint).normalized, Color.red);
            
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
        }
        
        return false;
    }
}
