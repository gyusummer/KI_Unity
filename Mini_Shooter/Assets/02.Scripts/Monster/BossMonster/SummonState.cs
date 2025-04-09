using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SummonState : BossState    
{
    //소환 상태에 진입을 하면
    //애니메이션을 재생해야 하며
    
    //몬스터를 소환해야함
    //  최소 두마리 ~ 4마리 정도의 개체를 소환
    //  플레이어 주변의 n미터 반경에 랜덤한 포지션 위치에서 소환
    
    //보스는 소환만 하고 자신의 상태를 진행함

    public override StateName Name => StateName.SummonState;
    
    [SerializeField] private Insect minionPrefab;
    [SerializeField] private float spawnRadius = 3.0f;

    [SerializeField] [Range(0.0f, 1.0f)]private float spawnTiming;
    
    private Animator animator;
    private BossMonster bossMonster;
    
    public override void Initialize(BossMonster bossMonster)
    {
        animator = bossMonster.animator;
        this.bossMonster = bossMonster;
    }

    private void Update()
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        
        if (currentState.IsName(AnimatorStateName) == false) return;

        if (currentState.normalizedTime > ExitTime)
        {
            bossMonster.ChangeState(StateName.IdleState);
        }
    }

    public override void Enter()
    {
        StartCoroutine(SpawnCoroutine());
    }

    public override void Exit()
    {
        
    }

    private IEnumerator SpawnCoroutine()
    {
        //애니메이션 재생중에 소환 타이밍이 도래할때까지 기다렸다가
        //소환 로직을 동작시키는 코루틴
        
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorStateName) == false)
        {
            yield return null;
        }
        
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < spawnTiming)
        {
            yield return null;
        }
        
        //소환 로직
        //몬스터를 소환해야함
        //  최소 두마리 ~ 4마리 정도의 개체를 소환
        //  플레이어 주변의 n미터 반경에 랜덤한 포지션 위치에서 소환

        int minionCount = Random.Range(2, 4);

        for (int i = 0; i < minionCount; i++)
        {
            Vector3 spawnPosition = Player.LocalPlayer.transform.position;
            
            Vector2 circlePoint = Random.insideUnitCircle;

            spawnPosition.x += spawnRadius * circlePoint.x;
            spawnPosition.y += Random.Range(1, 5);
            spawnPosition.z += spawnRadius * circlePoint.y;

            Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
