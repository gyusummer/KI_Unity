using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : MonoBehaviour
{
    public enum StateName
    {
        IdleState,
        ScratchState,
        BreathState,
        SummonState,
        DeadState,
    }

    public abstract StateName Name { get; }

    public string AnimatorStateName;
    public float ExitTime;
    
    public abstract void Initialize(BossMonster bossMonster);
    public abstract void Enter();
    public abstract void Exit();
}
