using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

public class PlayerCharacter : MonoBehaviour
{
    private static readonly int ATTACK = Animator.StringToHash("Attack");
    private static readonly int MOVE_X = Animator.StringToHash("MoveX");
    private static readonly int MOVE_Z = Animator.StringToHash("MoveZ");
    private static readonly int SWAP = Animator.StringToHash("Swap");
    private static readonly int LOCOMOTION = Animator.StringToHash("Locomotion");
    private Animator animator;
    private AnimatorStateInfo currentStateInfo;
    private bool isInTransition = false;
    
    [Header("Weapon Swap")]
    public RuntimeAnimatorController unarmedAnimatorController;
    public RuntimeAnimatorController twoHandSwordAnimatorController;
    public GameObject twoHandSword;
    
    private Vector2 moveInput;
    private bool inputAttack;

    public float baseSpeed;
    public float runMultiplier = 2;
    private float targetSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        isInTransition = animator.IsInTransition(0);
        PreProMoveInput();
        Move();
        inputAttack = Input.GetKeyDown(KeyCode.Mouse0);
        if (inputAttack && !isInTransition)
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentStateInfo.shortNameHash == LOCOMOTION && !isInTransition)
            {
                SwapWeapon();
            }
        }
    }
    private void SwapWeapon()
    {
        animator.SetTrigger(SWAP);
        Invoke("ToggleWeapon", 0.7f);
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
    private void Attack()
    {
        bool isAttack1 = currentStateInfo.IsName("Attack-1");
        bool isAttack2 = currentStateInfo.IsName("Attack-2");
        bool isAttack3 = currentStateInfo.IsName("Attack-3");
        bool isAttacking = isAttack1 || isAttack2 || isAttack3;

        if (inputAttack && isAttack3 == false)
        {
            float normalizedTime = currentStateInfo.normalizedTime;
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
    [Range(0, 1)] public float inputLerpT = 0.1f;
    private void PreProMoveInput()
    {
        Vector2 curMove = new Vector2(animator.GetFloat(MOVE_X), animator.GetFloat(MOVE_Z));
        Vector2 rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 targetMove = rawInput.normalized;
        if (Input.GetKey(KeyCode.LeftShift))
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

        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * (targetSpeed * Time.deltaTime));
    }
}