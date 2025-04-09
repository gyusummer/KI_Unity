using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectBullet : MonoBehaviour
{
    //생성되면 플레이어 쪽으로 날아가서
    //충돌시 데미지를 입힌다.
    //충돌을 하지 않으면 n초뒤에 사라진다.

    [System.Serializable]
    public class BulletData
    {
        public float speed;
        public int damage;
        public float deathTime;
    }

    public Insect Owner { get; set; }

    [SerializeField] private BulletData bulletData;
    private float currentTime = 0.0f;
    
    
    private void OnEnable()
    {
        currentTime = 0;

        var targetPosition = Player.LocalPlayer.transform.position + Vector3.up * 1.7f;
        var direction = (targetPosition - transform.position).normalized;
        transform.forward = direction;
    }

    private void Update()
    {
        transform.Translate(transform.forward * (Time.deltaTime * bulletData.speed), Space.World);
        currentTime += Time.deltaTime;
        if (currentTime >= bulletData.deathTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(Player.LocalPlayer.MainCollider) == false) return;
        
        //데미지 처리
        CombatEvent e = new CombatEvent();
        e.Damage = bulletData.damage;
        e.HitPosition = other.ClosestPoint(transform.position);
        e.Sender = Owner;
        e.Receiver = Player.LocalPlayer;

        CombatSystem.Instance.AddInGameEvent(e);
    }
}
