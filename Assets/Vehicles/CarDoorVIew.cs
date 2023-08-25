using System;
using System.Collections;
using UnityEngine;

namespace Vehicles
{
    public class CarDoorVIew : MonoBehaviour
    {
        
        private Quaternion _doorBoneDefaultRotation;
        private DoorState _doorState;
        private Coroutine _openDoorCoroutine = null;

        private Transform _car;
        
        private void Awake()
        {
            _car = transform.root;
            _doorBoneDefaultRotation = transform.localRotation;
        }

        public void SetDoorOpen()
        {
            _doorState = DoorState.Physical;
        }

        public void SetDoorClose()
        {
            _doorState = DoorState.Closed;
            transform.rotation = _doorBoneDefaultRotation;
        }

        public void SetDoorFollowTarget(Transform target)
        {
            if (_openDoorCoroutine != null)
            {
                StopCoroutine(_openDoorCoroutine);
            }
            
            if (!target)
            {
                _doorState = DoorState.Physical;
                _openDoorCoroutine = StartCoroutine(PhysicalDoorBehavior());

                return;
            }

            _doorState = DoorState.FollowTarget;
            _openDoorCoroutine = StartCoroutine(DoorFollowTarget(target));
        }

        private IEnumerator PhysicalDoorBehavior()//TODO
        {
            yield return null;
        }
        
        private IEnumerator DoorFollowTarget(Transform target)
        {
            while (true)
            {
                float distanceToPlane = Vector3.Dot(_car.up, target.position - transform.position);
                Vector3 pointOnPlane = target.position - (_car.up * distanceToPlane);
               
                transform.rotation = Quaternion.LookRotation(pointOnPlane - transform.position, _car.up) * _doorBoneDefaultRotation;

                yield return null;
            }
        }

        private enum DoorState
        {
            FollowTarget,
            Physical,
            Closed
        }
    }
}
