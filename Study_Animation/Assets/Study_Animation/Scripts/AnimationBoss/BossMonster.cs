using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossMonster : MonoBehaviour, IClickable
{
    private static readonly int SWING = Animator.StringToHash("Swing");
    private static readonly int BREATH = Animator.StringToHash("Breath");
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int GET_HIT = Animator.StringToHash("GetHit");
    private static readonly int DEAD = Animator.StringToHash("Dead");
    private Animator animator;
    private AnimatorStateInfo info;
    
    public ParticleSystem breathParticle;

    private int curHp = 10;
    public int damageThreshold = 2;
    private int damageStack = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        if (animator.IsInTransition(0))
        {
            breathParticle.Stop();
        }
        if (info.shortNameHash == IDLE && !animator.IsInTransition(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        int pattern = Random.Range(0, 2);
        switch (pattern)
        {
            case 0:
                animator.SetTrigger(SWING);
                break;
            case 1:
                animator.SetTrigger(BREATH);
                breathParticle.Play();
                break;
            // default:
            //     animator.SetTrigger(SWING);
            //     break;
        }
    }

    public void OnClick()
    {
        GetDamage();
    }

    private void GetDamage()
    {
        if (curHp <= 0) return;
        curHp -= 1;
        damageStack++;
        if (curHp <= 0)
        {
            animator.SetTrigger(DEAD);
        }
        else if (damageStack >= damageThreshold)
        {
            damageStack = 0;
            animator.SetTrigger(GET_HIT);
        }
    }
}
