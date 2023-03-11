using UnityEngine;

namespace Character.Scripts.Npc.StateMachine
{
    public class NpcScaredRunningToCover : StateMachineBehaviour
    {
        private NpcCharacterController _npcController;
        private NpcAnimations _npcAnimations;
        private float _walkSpeed;
        private Vector3 _currentTarget;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _walkSpeed = 1f;
            _npcController = animator.transform.parent.GetComponent<NpcCharacterController>();
            _npcController.ChangeAiming(false);
            _npcAnimations = _npcController.characterAnimations as NpcAnimations;
            GetNewTarget();
        }

        //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float distance = Vector3.Distance(_npcController.transform.position, _currentTarget);
            
            if (distance < 0.4f)
            {
               GetNewTarget();
            }
            
            Debug.DrawLine(_npcController.transform.position, _currentTarget, Color.cyan);

            _npcAnimations.StandardLocomotion(_currentTarget, _walkSpeed);
        }

        private void GetNewTarget()
        {
            if (_npcController.currentRoute.Count == 0)
            {
                _npcController.OnCoverArrived();
                return;
            }

            _currentTarget = _npcController.currentRoute[_npcController.currentRoute.Count - 1].GetPosition();
            _npcController.currentRoute.RemoveAt(_npcController.currentRoute.Count - 1);
        }

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
