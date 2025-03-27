using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mine
{
    public class AttackState : PlayerState
    {
        private int combo = 0;
        private void Combo()
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
