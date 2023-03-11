using UnityEngine;

namespace Character.Scripts.Npc.StateMachine
{
    public class NpcScaredHiding : StateMachineBehaviour
    {
        private NpcCharacterController _npcController;
        private NpcAnimations _npcAnimations;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _npcController = animator.transform.parent.GetComponent<NpcCharacterController>();
            _npcController.ChangeAiming(false);
            _npcAnimations = _npcController.characterAnimations as NpcAnimations;
            
            _npcAnimations.ScaredHiding();
        }

        //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //}
        

        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
            
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
