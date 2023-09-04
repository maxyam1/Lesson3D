using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;
using Weapons;

namespace Character.Scripts
{
    public abstract class AbstractCharacterAnimations : MonoBehaviour
    {
        [SerializeField] protected AbstractCharacterController characterController;
        [SerializeField] protected Animator animator;
        [SerializeField] protected float rotationSpeed = 10;
        [SerializeField] protected Transform targetLook;

        [SerializeField] protected Vector3 carSitAnimationPosDelta;
        [SerializeField] protected Quaternion carSitAnimationRotDelta;
        
        protected Transform leftHand;
        
        protected bool isAiming;
        protected bool isGrounded;

        protected Transform weaponRotationPivot;
        protected Transform weaponLeftHandTarget;
        protected Transform weaponRightHandTarget;
        protected float leftHandWeight;
        protected float rightHandWeight;
        
        protected Vector3 leftHandTargetPos;
        protected Quaternion leftHandTargetRot;
        protected Vector3 rightHandTargetPos;
        protected Quaternion rightHandTargetRot;
        protected Weapon currentWeapon;


        protected int id_horizontal;
        protected int id_vertical;
        protected int id_isAiming;
        protected int id_isGrounded;
        protected int id_putGun;
        protected int id_takeGun;
        protected int id_reload;
        protected int id_jump;
        protected int id_enteringCar;
        protected int id_exitCar;
        

        protected virtual void OnEnable()
        {
            characterController.inventory.OnCurrentWeaponChanged += OnWeaponChanged;
            currentWeapon = characterController.inventory.currentWeapon;
            
            id_isAiming = Animator.StringToHash("isAiming");
            id_isGrounded = Animator.StringToHash("isGrounded");
            id_putGun = Animator.StringToHash("putGun");
            id_takeGun = Animator.StringToHash("takeGun");
            id_reload = Animator.StringToHash("reload");
            id_jump = Animator.StringToHash("jump");
            id_horizontal = Animator.StringToHash("horizontal");
            id_vertical = Animator.StringToHash("vertical");
            id_enteringCar = Animator.StringToHash("enteringCar");
            id_exitCar = Animator.StringToHash("exitCar");
        }

        protected virtual void OnDisable()
        {
            characterController.inventory.OnCurrentWeaponChanged -= OnWeaponChanged;
        }

        private void Awake()
        {
            leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            
            if (weaponRotationPivot == null)
            {
                weaponRotationPivot = new GameObject().transform;
                weaponRotationPivot.name = "weapon rotation pivot";
                weaponRotationPivot.parent = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            }

            if (weaponRightHandTarget == null)
            {
                weaponRightHandTarget = new GameObject().transform;
                weaponRightHandTarget.name = "right hand target";
                weaponRightHandTarget.parent = weaponRotationPivot;   
            }
        }

        private void OnWeaponChanged(Weapon currentWeapon)
        {
            this.currentWeapon = currentWeapon;
            
            if (currentWeapon == null)
            {
                weaponLeftHandTarget = null;
            }
            else
            {
                weaponRotationPivot.localPosition = currentWeapon.rotationPivotPos;
                weaponLeftHandTarget = currentWeapon.leftHandTarget;
                weaponRightHandTarget.localPosition = currentWeapon.rightHandPos;
                weaponRightHandTarget.localRotation = Quaternion.Euler(currentWeapon.rightHandRot);   
            }
        }

        private void SetLeftHandWeight(float weight)
        {
            leftHandWeight = weight;
        }
        
        private void SetLeftHandWeight(float weight, float duration)
        {
            DOTween.To(()=>leftHandWeight, (value)=>leftHandWeight = value, weight, duration);
        }
        
        private void SetLeftHandWeight(float weight, float duration, Action onComplete)
        {
            DOTween.To(()=>leftHandWeight, (value)=>leftHandWeight = value, weight, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        private void SetRightHandWeight(float weight, float duration)
        {
            DOTween.To(()=>rightHandWeight, (value)=>rightHandWeight = value, weight, duration);
        }
        
        private void SetRightHandWeight(float weight, float duration, Action onComplete)
        {
            DOTween.To(()=>rightHandWeight, (value)=>rightHandWeight = value, weight, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        public void SetAiming(bool isAiming)
        {
            this.isAiming = isAiming;
            animator.SetBool(id_isAiming, isAiming);

            if(isReloading)
                return;
                
            if (isAiming)
            {
                SetLeftHandWeight(1, 0.5f);
                SetRightHandWeight(1, 0.5f);
            }
            else
            {
                SetLeftHandWeight(0, 0.5f);
                SetRightHandWeight(0, 0.5f);
            }
        }

        protected abstract void StandardLocomotion(float horizontal, float vertical);

        protected abstract void AimingLocomotion(float horizontal, float vertical);

        public void Locomotion(float horizontal, float vertical)
        {
            if (isAiming)
            {
                AimingLocomotion(horizontal, vertical);
            }
            else
            {
                StandardLocomotion(horizontal, vertical);
            }
        }

        public void SetIsGrounded(bool isGrounded)
        {
            if (!this.isGrounded && isGrounded)
            {
                this.isGrounded = true;
                animator.SetBool(id_isGrounded,true);
            }
            else if(this.isGrounded && !isGrounded)
            {
                this.isGrounded = false;
                animator.SetBool(id_isGrounded,false);
            }
        }

        public void SetJump()
        {
            animator.SetTrigger(id_jump);
        }


        private void Update()
        {
            // Если делать это напрямую у рук появляется тряска и не точность
            if (weaponLeftHandTarget)
            {
               // leftHandTargetPos = weaponLeftHandTarget.position;
               // leftHandTargetRot = weaponLeftHandTarget.rotation;
               // rightHandTargetPos = weaponRightHandTarget.position;
               // rightHandTargetRot = weaponRightHandTarget.rotation;

               Transform leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
               
               leftHandTargetPos = new Vector3(weaponLeftHandTarget.lo leftHand.position.y)//////КОРОЧЕ НАДО ДЕЛАТЬ ТАК: АНИМАЦИЯ ДВЕРИ ЧЕРЕЗ CURVE, ЧЕРЕЗ IK КЛАСТЬ РУКУ НА РУЧКУ ДВЕРИ 
            }
        }

        public void PutGun()
        {
            animator.SetTrigger(id_putGun);
        }

        public void TakeGun()
        {
            animator.SetTrigger(id_takeGun);
        }

        //IK
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (targetLook == null)
            {
                return;
            }

            switch (characterController.CurrentStatus)
            {
                case AbstractCharacterController.CharacterStatus.Driving:
                    if (isAiming)
                    {
                        //animator.SetLookAtWeight(1f, 0f, 1f);
                    }
                    else
                    {
                        animator.SetLookAtWeight(0.3f, 0f, 0.3f);
                    }
                    break;
                
                case AbstractCharacterController.CharacterStatus.Standard:
                    if (isAiming)
                    {
                        animator.SetLookAtWeight(1f, 0f, 1f);
                    }
                    else
                    {
                        animator.SetLookAtWeight(0.3f, 0.3f, 0.3f);
                    } 
                    break;
            }

            animator.SetLookAtPosition(targetLook.position);
            
            if (weaponLeftHandTarget != null)
            {
                weaponRotationPivot.LookAt(targetLook);
                
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTargetPos);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTargetRot);   
                
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTargetPos);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTargetRot);
            }
        }

        #region ===Reloading===

        protected GameObject currentSpawnedMag;
        protected bool isReloading;
        public void Reload()
        {
            isReloading = true;
            weaponLeftHandTarget = currentWeapon.magHandTarget;
            currentWeapon.StartReload();
            animator.SetTrigger(id_reload);
            
            SetLeftHandWeight(1,0.3f);
            SetRightHandWeight(0,0.3f);
        }
        
        public void GrabMag()
        {
            currentWeapon.magOnWeapon.SetActive(false);
            currentSpawnedMag = Instantiate(currentWeapon.magPrefab, leftHand.position, leftHand.rotation, leftHand);
            SetLeftHandWeight(0,0.1f);
        }
        
        public void DropMag()
        {
            currentSpawnedMag.transform.SetParent(null);
            currentSpawnedMag.AddComponent<Rigidbody>();
            Destroy(currentSpawnedMag, 10f);
            currentSpawnedMag = null;
        }

        protected GameObject newSpawnedMag;
        public void TakeNewMag()
        {
            newSpawnedMag = Instantiate(currentWeapon.magPrefab, leftHand.position, leftHand.rotation, leftHand);
            SetLeftHandWeight(1,0.3f);
        }
        
        public void PutNewMag()
        {
            currentWeapon.magOnWeapon.SetActive(true);
            Destroy(newSpawnedMag);
            newSpawnedMag = null;
            SetLeftHandWeight(0,0.2f);
        }

        public void PullBolt()
        {
            currentWeapon.BoltPulled();
            weaponLeftHandTarget = currentWeapon.leftHandTarget;
        }

        public void ReloadFinished()
        {
            isReloading = false;
            currentWeapon.ReloadFinished();
            
            if (isAiming)
            {
                SetLeftHandWeight(1,0.3f);
                SetRightHandWeight(1,0.3f);
            }
            else
            {
                SetLeftHandWeight(1,0.3f, () => SetLeftHandWeight(0, 0.3f));
            }
            
        }

        #endregion

        #region ==CarSitting==

        private Action _onEnteringAnimFinished;
        private Action _onExitAnimFinished;
        private CarController _car;
        public void EnterInCar(CarController car, Action onEnteringAnimFinished)
        {
            _car = car;
            
            transform.SetParent(car.Seat);
            transform.localPosition = carSitAnimationPosDelta;
            transform.localRotation = carSitAnimationRotDelta;
            
            _onEnteringAnimFinished = onEnteringAnimFinished;
            animator.SetFloat(id_horizontal,0);
            animator.SetFloat(id_vertical,0);
            animator.SetTrigger(id_enteringCar);
        }

        public void FinallyEnterCar()
        {
            _onEnteringAnimFinished?.Invoke();
            _onEnteringAnimFinished = null;
            //_car = null;
        }

        public void GrabDoor(CarDoorVIew.HandleType handle)
        {
            var door = _car.CarView.GetDoor(CarDoor.Left);
            if (door == null)
            {
                return;
            }
            
            door.SetDoorOpen();
            door.SetDoorFollowTarget(animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal), handle);
            
            weaponLeftHandTarget = door.GetHandle(handle);
            SetLeftHandWeight(1);
        }

        public void UnGrabDoor()
        {
            var door = _car.CarView.GetDoor(CarDoor.Left);
            if (door == null)
            {
                return;
            }
            
            door.SetDoorFollowTarget(null, null);
            
            SetLeftHandWeight(0);
        }

        public void CloseDoor()
        {
            var door = _car.CarView.GetDoor(CarDoor.Left);
            if (door == null)
            {
                return;
            }
            
            door.SetDoorFollowTarget(null, null);
            door.SetDoorClose(0.25f);
        }


        public void ExitFromCar(Action onFinallyExit)
        {
            _onExitAnimFinished = onFinallyExit;
            animator.SetTrigger(id_exitCar);
            //animator.SetBool(id_sitingInCar,false);
        }

        public void FinallyExitCar()
        {
            _onExitAnimFinished?.Invoke();
            _onExitAnimFinished = null;
        }

        #endregion
    }
}
