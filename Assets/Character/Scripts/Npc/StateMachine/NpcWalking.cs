using System;
using UnityEngine;
using Waypoint_system;
using Random = UnityEngine.Random;

namespace Character.Scripts.Npc.StateMachine
{
    public class NpcWalking : StateMachineBehaviour
    {
        private NpcCharacterController _npcController;
        private NpcAnimations _npcAnimations;
        private float _walkSpeed;
        private Vector3 _currentTarget;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _walkSpeed = Random.Range(0.4f, 0.6f);
            _npcController = animator.transform.parent.GetComponent<NpcCharacterController>();
            _npcController.ChangeAiming(false);
            _currentTarget = _npcController.currentWaypoint.GetPosition();
            _npcAnimations = _npcController.characterAnimations as NpcAnimations;
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
            if (_npcController.isMovingForward)
            {
                if (_npcController.currentWaypoint.next != null)
                {
                    _npcController.currentWaypoint = _npcController.currentWaypoint.next;
                }
                else if(_npcController.currentWaypoint.prev != null)
                {
                    _npcController.currentWaypoint = _npcController.currentWaypoint.prev;
                    _npcController.isMovingForward = !_npcController.isMovingForward;
                }
            }
            else
            {
                if (_npcController.currentWaypoint.prev != null)
                {
                    _npcController.currentWaypoint = _npcController.currentWaypoint.prev;
                }
                else if(_npcController.currentWaypoint.next != null)
                {
                    _npcController.currentWaypoint = _npcController.currentWaypoint.next;
                    _npcController.isMovingForward = !_npcController.isMovingForward;
                }
            }
            
            _currentTarget = _npcController.currentWaypoint.GetPosition();
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
