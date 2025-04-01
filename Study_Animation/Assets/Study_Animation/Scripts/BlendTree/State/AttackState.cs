using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mine
{
    public class AttackState : PlayerState
    {
        private int combo = 0;
        
        public override void Enter()
        {
            // 구독
        }

        public override void Exit()
        {
            // 구독 취소
        }
        
        private void Attack()
        {
            combo++;
            if (combo < 3)
            {
                float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (normalizedTime is >= 0.4f and <= 0.85f)
                {
                    animator.SetTrigger(ATTACK);
                }
            }
        }
    }
}
