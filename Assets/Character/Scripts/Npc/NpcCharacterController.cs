using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Waypoint_system;

namespace Character.Scripts
{
    public interface IScareableByShot
    {
        public void Scare(Vector3 shotPosition);
    }

    public class NpcCharacterController : AbstractCharacterController, IScareableByShot
    {
        public Waypoint currentWaypoint;
        public bool isMovingForward;
        public List<Waypoint> way;
        private bool _isAiming;

        [SerializeField] private Animator aiTree;

        private void OnDestroy()
        {
            Destroy(aiTree);
        }

        public void ChangeAiming(bool isAiming)
        {
            characterAnimations.SetAiming(isAiming);
            _isAiming = isAiming;
        }

        public void Scare(Vector3 shotPosition)
        {
            throw new NotImplementedException();
        }
    }
}
