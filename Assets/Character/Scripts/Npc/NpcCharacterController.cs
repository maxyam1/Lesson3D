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
        public List<Waypoint> currentRoute;
        private bool _isAiming;

        private bool _isScared;

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
            if (_isScared)
            {
                return;
            }


            _isScared = true;
            Cover[] covers = GameObject.FindObjectsOfType<Cover>();

            Cover closestCover = null;
            float minDistance = float.MaxValue;
            
            foreach (var cover in covers)
            {
                float distance = Vector3.Distance(cover.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCover = cover;
                }
            }

            if (closestCover != null)
            {
                WaypointRouteFinder.FindRoute(out currentRoute, currentWaypoint, closestCover);
                aiTree.SetBool("safeSpotFound",true);
            }
            else
            {
                aiTree.SetBool("safeSpotFound",false);
            }
            
            aiTree.SetBool("scared", true);
        }

        public void OnCoverArrived()
        {
            aiTree.SetBool("hided", true);
        }
    }
}
