using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    public Vector3 offset;
    private Animator animator;

    public Transform leftKnee;
    public Transform rightKnee;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        HandleFootIK(AvatarIKGoal.LeftFoot, AvatarIKHint.LeftKnee, leftKnee);
        HandleFootIK(AvatarIKGoal.RightFoot, AvatarIKHint.RightKnee, rightKnee);
    }

    private void HandleFootIK(AvatarIKGoal goal, AvatarIKHint hint, Transform knee)
    {
        Vector3 footIkGoalPosition = animator.GetIKPosition(goal);
        Ray ray = new Ray(footIkGoalPosition + Vector3.up * 10f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, LayerMask.GetMask("Ground")))
        {
            Vector3 point =  footIkGoalPosition;
            point.y = hit.point.y;
            
            animator.SetIKPosition(goal, point + offset);
            animator.SetIKPositionWeight(goal, 1.0f);
            
            animator.SetIKRotation(goal, Quaternion.LookRotation(transform.forward, hit.normal));
            animator.SetIKRotationWeight(goal, 1.0f);

            if (knee != null)
            {
                animator.SetIKHintPosition(hint, knee.position);
                animator.SetIKHintPositionWeight(hint, 1.0f);
            }
        }
        else
        {
            animator.SetIKPositionWeight(goal, 0.0f);
            animator.SetIKRotationWeight(goal, 0.0f);
            animator.SetIKHintPositionWeight(hint, 0.0f);
        }

    }
}
