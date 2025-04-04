using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathState : BossState
{
    [Header("Breath Ref")]
    public Breath breath;
    public Transform breathPoint;

    private Animator animator;
    private BossMonster bossMonster;
    
    public override StateName Name => StateName.BreathState;
    
    public override void Initialize(BossMonster bossMonster)
    {
        animator = bossMonster.Animator;
        this.bossMonster = bossMonster;
        
        breath.SetProperty(breathPoint, transform);
        breath.gameObject.SetActive(false);
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
        breath.ResetColliders();
        breath.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        breath.gameObject.SetActive(false);
    }
}
