using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleState : BossState
{
    private int[] bossAttacks;
    private Animator animator;
    private BossMonster bossMonster;
    
    public override StateName Name => StateName.IdleState;

    public override void Initialize(BossMonster bossMonster)
    {
        this.bossMonster = bossMonster;
        bossAttacks = new[] { BossMonster.SCRATCH, BossMonster.BREATH};
        animator = bossMonster.animator;
    }

    private void Update()
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName(AnimatorStateName) == false) return;

        if (currentState.normalizedTime > ExitTime)
        {
            int nextAttackTrigger = Random.Range(0, bossAttacks.Length); // 0 => 스크래치, 1 => 브레스
            int stateValue = nextAttackTrigger + 1; // 1 => 스크래치 상태, 2 => 브레스 상태
            animator.SetTrigger(bossAttacks[nextAttackTrigger]);
            bossMonster.ChangeState((StateName)stateValue);
        }
    }
    
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

  
}
