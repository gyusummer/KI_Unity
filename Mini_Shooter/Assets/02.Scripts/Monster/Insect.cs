using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour, IDamageAble
{
    private static readonly int ATTACK = Animator.StringToHash("Attack");

    private static readonly int DEATH = Animator.StringToHash("Death");
    // 보스의 패턴중에 소환되며,
    // 소환시에 LocalPlayer와의 위치 계산을 한 후
    // 공격 범위 안에 있으면 공격을
    // 공격 범위 밖에 있으면 이동(플레이어 쪽으로)을 함.

    [System.Serializable]
    public class MonsterStat
    {
        public int hp;
        public int maxHp;
        public float range;
        public float speed;
    }

    [System.Serializable]
    public class AttackAnimEvent
    {
        [Range(0.0f, 1.0f)] public float timing;
        public bool isAttacked = false;
    }
    
    public Collider MainCollider => collider;
    public GameObject GameObject => gameObject;

    
    [SerializeField] private MonsterStat monsterStat;
    [SerializeField] private AttackAnimEvent fireAnimEvent;
    [SerializeField] private Collider collider;
    [SerializeField] private Animator animator;

    [SerializeField] private InsectBullet bulletPrefab;
    [SerializeField] private Transform firePoint;

    
    
    private Transform localPlayerTransform;
    
    private AnimatorStateInfo prevInfo;
    
    private void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
        localPlayerTransform = Player.LocalPlayer.transform;
    }

    private void OnEnable()
    {
        monsterStat.hp = monsterStat.maxHp;
    }
    
    private void Update()
    {
        // 보스의 패턴중에 소환되며,
        // 소환시에 LocalPlayer와의 위치 계산을 한 후
        // 공격 범위 안에 있으면 공격을
        // 공격 범위 밖에 있으면 이동(플레이어 쪽으로)을 함.
        
        float distance = Vector3.Distance(localPlayerTransform.position, transform.position);
        Vector3 direction = (localPlayerTransform.position - transform.position).normalized;
        transform.forward = direction;

        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (currentInfo.IsName("Move"))
        {
            if (monsterStat.range >= distance)// 공격 범위 안임?
            {
                animator.ResetTrigger(ATTACK);
                animator.SetTrigger(ATTACK);
            }
            else
            {
                transform.Translate(direction * (Time.deltaTime * monsterStat.speed), Space.World);
            }
        }
        else if (currentInfo.IsName("Attack"))
        {
            //공격애니메이션 재생중에 특정 타이밍에 불렛을 유저에게 쏜다
            //쐈는지?
            //현재타이밍 계산(맞다면 발사, 아니라면 발사 안함)

            //Move -> Attack으로 첫 진입
            if (prevInfo.shortNameHash != currentInfo.shortNameHash)
            {
                fireAnimEvent.isAttacked = false;
            }
            
            if (fireAnimEvent.isAttacked == false && 
                fireAnimEvent.timing < currentInfo.normalizedTime) //normalizedTime이 timing보다 커지면
            {
                fireAnimEvent.isAttacked = true;
                
                //밑에부터는 발사로직
                var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.Owner = this;
            }
        }
        else
        {
            transform.Translate(Physics.gravity * Time.deltaTime, Space.World);
        }

        prevInfo = currentInfo;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        monsterStat.hp -= combatEvent.Damage;
        if (monsterStat.hp <= 0)
        {
            animator.SetTrigger(DEATH);
        }
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        
    }
}
