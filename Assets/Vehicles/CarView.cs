using System;
using UnityEngine;

namespace Vehicles
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private SuspensionType frontSuspensionType;
        [SerializeField] private SuspensionType rearSuspensionType;
        
        [SerializeField] private Transform wheelRenderer_FL;
        [SerializeField] private Transform wheelRenderer_FR;
        [SerializeField] private Transform wheelRenderer_RL;
        [SerializeField] private Transform wheelRenderer_RR;

        [SerializeField] private Transform leftDoorBone;
        private Quaternion _leftDoorBoneDefaultRotation;
        
        [SerializeField] private Transform rightDoorBone;
        private Quaternion _rightDoorBoneDefaultRotation;
        
        public void Start()
        {
            _leftDoorBoneDefaultRotation = leftDoorBone.localRotation;
            _rightDoorBoneDefaultRotation = rightDoorBone.localRotation;
        }

        public void SetWheelRenderersPosition()
        {
        }

        public void SetDoorOpen(CarDoor carDoor)
        {
            
        }

        public void SetDoorClose(CarDoor carDoor)
        {
            Transform targetDoorBone = null;
            Quaternion defaultRotation; 
            
            switch (carDoor)
            {
                case CarDoor.Left:
                    targetDoorBone = leftDoorBone;
                    defaultRotation = _leftDoorBoneDefaultRotation;
                    break;
                case CarDoor.Right:
                    targetDoorBone = rightDoorBone;
                    defaultRotation = _rightDoorBoneDefaultRotation;
                    break;
                default:
                    defaultRotation = new Quaternion();
                    break;
            }

            targetDoorBone.rotation = defaultRotation;
        }
    }
    
    public enum SuspensionType
    {
        Dependent,
        Independent
    }

    public enum CarDoor
    {
        Left,
        Right
    }
}
