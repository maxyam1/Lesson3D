using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Utils.SerializableDict;

namespace Vehicles
{
    public class CarDoorVIew : MonoBehaviour
    {
        private float _minAngle;
        [SerializeField] private float maxAngle;

        [SerializeField] private SerializableDictionary<HandleType, Transform> handles;

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

        public void SetDoorClose(float interpolateTIme)
        {
            _doorState = DoorState.Closed;
            
            transform.DOLocalRotate(_doorBoneDefaultRotation.eulerAngles, interpolateTIme).SetEase(Ease.Linear)
                .OnComplete(() => transform.localRotation = _doorBoneDefaultRotation);
        }

        public void SetDoorFollowTarget(Transform target, HandleType? handleType)
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
            _openDoorCoroutine = StartCoroutine(DoorFollowTarget(target, handleType));
        }

        private IEnumerator PhysicalDoorBehavior()//TODO
        {
            yield return null;
        }
        
        private IEnumerator DoorFollowTarget(Transform target, HandleType? handleType)
        {

            while (true)
            {
                Vector3 dirToHandle = Vector3.zero;
                
                if (handleType.HasValue)
                {
                    if (handles.ContainsKey(handleType.Value))
                    {
                        dirToHandle = handles[handleType.Value].localPosition;
                        //dirToHandle.y = 0;
                    }
                }

                float angleToHandle = Mathf.Atan(dirToHandle.x / dirToHandle.y) * Mathf.Rad2Deg;
                
                
                
                
                Vector3 targetPos = target.position;
                
                float distanceToPlane = Vector3.Dot(_car.up,targetPos  - transform.position);
                Vector3 pointOnPlane = targetPos - (_car.up * distanceToPlane);
               
                transform.rotation = Quaternion.LookRotation(pointOnPlane - transform.position, _car.up) * _doorBoneDefaultRotation;
                transform.Rotate(Vector3.forward, angleToHandle);

                ClampDoorRotation();
                
                yield return null;
            }
        }

        private void ClampDoorRotation()
        {
            Vector3 euler = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(euler.x, Mathf.Clamp(euler.y, _minAngle, _minAngle + maxAngle), euler.z);
        }
        

        private enum DoorState
        {
            FollowTarget,
            Physical,
            Closed
        }
        
        public enum HandleType
        {
            OutsideHandle,
            InsideHandle
        }
    }
}
