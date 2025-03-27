using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Mine
{
    public class LocomotionState : PlayerState
    {
        [Header("Weapon Swap")]
        public RuntimeAnimatorController unarmedAnimatorController;
        public RuntimeAnimatorController twoHandSwordAnimatorController;
        public GameObject twoHandSword;
        
        private Vector2 moveInput;
        public float baseSpeed;
        public float runMultiplier = 2;
        private float targetSpeed;
        
        private void SwapWeapon()
        {
            animator.SetTrigger(SWAP);
            Task.Delay(700).Wait();
            animator.runtimeAnimatorController = twoHandSwordAnimatorController;
        }
        private void ToggleWeapon()
        {
            if (twoHandSword.activeSelf)
            {
                animator.runtimeAnimatorController = unarmedAnimatorController;
                twoHandSword.SetActive(false);
            }
            else
            {
                animator.runtimeAnimatorController = twoHandSwordAnimatorController;
                twoHandSword.SetActive(true);
            }
        }
        [Range(0, 1)] public float inputLerpT = 0.1f;
        private void PreProMoveInput(Vector2 rawInput)
        {
            Vector2 curMove = new Vector2(animator.GetFloat(MOVE_X), animator.GetFloat(MOVE_Z));
            Vector2 targetMove = rawInput.normalized;
            if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
            {
                targetMove *= 2;
                targetSpeed = baseSpeed * runMultiplier;
            }
            else
            {
                targetSpeed = baseSpeed;
            }
    
            moveInput = Vector2.Lerp(curMove, targetMove, inputLerpT);
        }
        private void Move()
        {
            animator.SetFloat(MOVE_X, moveInput.x);
            animator.SetFloat(MOVE_Z, moveInput.y);
    
            player.transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * (targetSpeed * Time.deltaTime));
        }
    }
}

