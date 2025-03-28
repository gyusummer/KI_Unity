using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mine
{
    public abstract class PlayerState
    {
        protected static readonly int ATTACK = Animator.StringToHash("Attack");
        protected static readonly int MOVE_X = Animator.StringToHash("MoveX");
        protected static readonly int MOVE_Z = Animator.StringToHash("MoveZ");
        protected static readonly int SWAP = Animator.StringToHash("Swap");
        protected static readonly int LOCOMOTION = Animator.StringToHash("Locomotion");
        public static PlayerCharacter player;
        public static Animator animator;

        public abstract void Enter();
        public abstract void Exit();
    }
}

