using System;
using System.Collections;
using UnityEngine;

namespace Vehicles
{
    public class CarDoorVIew : MonoBehaviour
    {
        [SerializeField] private Transform car;
        [SerializeField] private Transform doorBone;
        private Quaternion _doorBoneDefaultRotation;
        private DoorState _doorState;
        private Coroutine _openDoorCoroutine = null; 
        
        //DEBUG

        public Transform Target;

        private void Start()
        {
            SetDoorOpen();
            SetDoorFollowTarget(Target);
        }

        //DEBUG
        
        
        private void Awake()
        {
            _doorBoneDefaultRotation = doorBone.localRotation;
        }

        public void SetDoorOpen()
        {
            _doorState = DoorState.Physical;
        }

        public void SetDoorClose()
        {
            _doorState = DoorState.Closed;
            doorBone.rotation = _doorBoneDefaultRotation;
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
                //Vector3 dir = target.position - doorBone.position;
                
                //doorBone.rotation = Quaternion.LookRotation(dir, Vector3.up) * _doorBoneDefaultRotation;
                //doorBone.localRotation = Quaternion.Euler(_doorBoneDefaultRotation.eulerAngles.x, doorBone.localRotation.eulerAngles.y, _doorBoneDefaultRotation.eulerAngles.z);
                
               
               float distanceToPlane = Vector3.Dot(car.up, target.position - doorBone.position);
               Vector3 pointOnPlane = target.position - (car.up * distanceToPlane);

               //doorBone.LookAt(pointOnPlane, doorBone.up);
               doorBone.rotation = Quaternion.LookRotation(pointOnPlane - doorBone.position, car.up) * _doorBoneDefaultRotation;
               
               //doorBone.rotation *= _doorBoneDefaultRotation;
               
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
