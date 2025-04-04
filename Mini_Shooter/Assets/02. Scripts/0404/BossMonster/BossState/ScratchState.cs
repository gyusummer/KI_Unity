using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchState : BossState
{
    [Header("Scratch Ref")]
    public Scratch scratch;
    
    private Animator animator;
    private BossMonster bossMonster;
    
    public override StateName Name => StateName.ScratchState;

    public override void Initialize(BossMonster bossMonster)
    {
        animator = bossMonster.Animator;
        this.bossMonster = bossMonster;
        
        scratch.gameObject.SetActive(false);
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
        scratch.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        scratch.gameObject.SetActive(false);
    }
}
