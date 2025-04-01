using System;
using System.Collections;
using System.Collections.Generic;
using Mine;
using UnityEngine;
using UnityEngine.Serialization;
using Input = UnityEngine.Input;

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
    public Equipment unarmed;
    public Equipment secondWeapon;
    public Equipment currentWeapon;
    [Header("Animation")]
    private Vector2 moveInput;
    private bool attackInput;
    [Header("Movement")]
    public float baseSpeed;
    public float runMultiplier = 2;
    private float targetSpeed;
    
    

    private void SwitchWeapon(Equipment equipment)
    {
        currentWeapon = equipment;
    }
    private void OnAnimatorIK(int layerIndex)
    {
        
        Transform rightHandTarget = currentWeapon.rightHandTarget;
        Transform leftHandTarget = currentWeapon.leftHandTarget;
        float ikWeight = currentWeapon.IKWeight;
        
        UpdateIK(AvatarIKGoal.LeftHand, leftHandTarget, ikWeight);
        UpdateIK(AvatarIKGoal.RightHand, rightHandTarget, ikWeight);

        void UpdateIK(AvatarIKGoal goal, Transform target, float ikWeight)
        {
            animator.SetIKRotation(goal, target.rotation);
            animator.SetIKRotationWeight(goal, ikWeight);
        
            animator.SetIKPosition(goal, target.position);
            animator.SetIKPositionWeight(goal, ikWeight);
        }
    }
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        SwitchWeapon(secondWeapon);
        
        PlayerState.player = this;
        PlayerState.animator = animator;
    }

    private void Update()
    {
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        isInTransition = animator.IsInTransition(0);
        PreProMoveInput();
        Move();
        attackInput = Input.GetKeyDown(KeyCode.Mouse0);
        if (attackInput && !isInTransition)
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

        if (attackInput && isAttack3 == false)
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