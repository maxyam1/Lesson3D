using System;
using System.Collections;
using UnityEngine;

namespace Vehicles
{
    public class CarDoorVIew : MonoBehaviour
    {
        private float _minAngle;
        [SerializeField] private float maxAngle;
        
        private Quaternion _doorBoneDefaultRotation;
        private DoorState _doorState;
        private Coroutine _openDoorCoroutine = null;

        private Transform _car;
        
        private void Awake()
        {
            _car = transform.root;
            _doorBoneDefaultRotation = transform.localRotation;
            _minAngle = _doorBoneDefaultRotation.eulerAngles.y;
        }

        public void SetDoorOpen()
        {
            _doorState = DoorState.Physical;
        }

        public void SetDoorClose()
        {
            _doorState = DoorState.Closed;
            transform.localRotation = _doorBoneDefaultRotation;
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

                ClampDoorRotation();
                
                yield return null;
            }
        }

        private void ClampDoorRotation()
        {
            Vector3 euler = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(euler.x, Mathf.Clamp(euler.y, _minAngle, _minAngle + maxAngle), euler.z);

            //Quaternion q = transform.localRotation;
            //
            //q.x /= q.w;
            //q.y /= q.w;
            //q.z /= q.w;
            //q.w = 1.0f;
            //
            //float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
            //angleY = Mathf.Clamp(angleY, _minAngle, maxAngle);
            //q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
            //
            //transform.localRotation = q.normalized;
        }
        

        private enum DoorState
        {
            FollowTarget,
            Physical,
            Closed
        }
    }
}
