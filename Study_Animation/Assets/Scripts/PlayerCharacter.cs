using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private static readonly int ATTACK = Animator.StringToHash("Attack");
    private static readonly int MOVE_X = Animator.StringToHash("MoveX");
    private static readonly int MOVE_Z = Animator.StringToHash("MoveZ");
    private Animator animator;
    public RuntimeAnimatorController anotherAnimatorController;

    public float Speed;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        LerpInput();
        Move();

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            animator.runtimeAnimatorController = anotherAnimatorController;
        }

        Attack();
    }

    private void Attack()
    {
        if (animator.IsInTransition(0)) return;

        var currentAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isAttack1 = currentAnimStateInfo.IsName("Attack-1");
        bool isAttack2 = currentAnimStateInfo.IsName("Attack-2");
        bool isAttack3 = currentAnimStateInfo.IsName("Attack-3");
        bool isAttacking = isAttack1 || isAttack2 || isAttack3;

        bool inputAttack = Input.GetKeyDown(KeyCode.Mouse0);
        if (inputAttack && isAttack3 == false)
        {
            float normalizedTime = currentAnimStateInfo.normalizedTime;
            if (isAttacking == false)
            {
                animator.SetTrigger(ATTACK);
            }
            else if (normalizedTime is >= 0.4f and <= 0.85f)
            {
                animator.SetTrigger(ATTACK);
            }
        }
    }

    [Range(0, 1)]
    public float inputLerpT;
    private void LerpInput()
    {
        Vector2 rawInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        Vector2 curMove = new Vector2(animator.GetFloat(MOVE_X), animator.GetFloat(MOVE_Z));
        moveInput = Vector2.Lerp(curMove, rawInput, inputLerpT);
    }

    private Vector2 moveInput;
    private void Move()
    {
        animator.SetFloat(MOVE_X, moveInput.x);
        animator.SetFloat(MOVE_Z, moveInput.y);
        
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * ((Speed * Time.deltaTime)));
    }
}
