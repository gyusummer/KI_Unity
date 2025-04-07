using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateEventSender : StateMachineBehaviour
{
    [System.Serializable]
    public class AnimStateEvent
    {
        public string name;
        [Range(0f, 1f)] public float time;
        public string parameter;
        public bool IsComplete { get; set; }
    }
    
    public AnimStateEvent[] events;
    private AnimStateEventListener eventListener;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //상태에 진입했을때 이벤트의 완료 상태를 초기화 해줌
        for (int i = 0; i < events.Length; i++)
        {
            events[i].IsComplete = false;
        }

        if (eventListener == null) eventListener = animator.GetComponent<AnimStateEventListener>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < events.Length; i++)
        {
            var eventData = events[i];
            if(eventData.IsComplete) continue;
            if(eventData.time > stateInfo.normalizedTime) continue; //진행시점이 도래하지 않았을 경우
            
            eventData.IsComplete = true;
            eventListener.OccursAnimStateEvent(eventData.name, eventData.parameter);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     
    // }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    // override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     // Implement code that processes and affects root motion
    // }
    //
    // // OnStateIK is called right after Animator.OnAnimatorIK()
    // override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     // Implement code that sets up animation IK (inverse kinematics)
    // }
}
