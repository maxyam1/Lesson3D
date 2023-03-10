using System;
using System.Collections.Generic;
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
        private Vector3 _prevTarget;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _walkSpeed = Random.Range(0.45f, 0.6f);
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
            Vector3 currentDirection = _currentTarget - _prevTarget;
            List<(float chance, Waypoint waypoint)> list = new List<(float chance, Waypoint waypoint)>();

            float totalChance = 0;
            
            foreach (var link in _npcController.currentWaypoint.links)
            {
                float chance = 180 - Vector3.Angle(currentDirection, link.transform.position - _currentTarget);
                totalChance += chance;
                list.Add((totalChance, link));
            }

            float randomResult = Random.Range(0, totalChance);

            Waypoint next = list[list.Count - 1].waypoint;
            
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (randomResult < list[i].chance)
                {
                    next = list[i].waypoint;
                    break;
                }
            }

            _prevTarget = _currentTarget;
            _npcController.currentWaypoint = next;
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
