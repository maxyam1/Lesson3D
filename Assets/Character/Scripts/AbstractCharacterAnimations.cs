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
        
        protected Transform leftHand;
        
        protected bool isAiming;
        protected bool isGrounded;

        protected Transform weaponRotationPivot;
        protected Transform leftHandTarget;
        protected Transform rightHandTarget;
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

            if (rightHandTarget == null)
            {
                rightHandTarget = new GameObject().transform;
                rightHandTarget.name = "right hand target";
                rightHandTarget.parent = weaponRotationPivot;   
            }
        }

        private void OnWeaponChanged(Weapon currentWeapon)
        {
            this.currentWeapon = currentWeapon;
            
            if (currentWeapon == null)
            {
                leftHandTarget = null;
            }
            else
            {
                weaponRotationPivot.localPosition = currentWeapon.rotationPivotPos;
                leftHandTarget = currentWeapon.leftHandTarget;
                rightHandTarget.localPosition = currentWeapon.rightHandPos;
                rightHandTarget.localRotation = Quaternion.Euler(currentWeapon.rightHandRot);   
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
            if (leftHandTarget)
            {
                leftHandTargetPos = leftHandTarget.position;
                leftHandTargetRot = leftHandTarget.rotation;

                rightHandTargetPos = rightHandTarget.position;
                rightHandTargetRot = rightHandTarget.rotation;
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

            if (characterController.InCar)
            {
                if (isAiming)
                {
                    //animator.SetLookAtWeight(1f, 0f, 1f);
                }
                else
                {
                    animator.SetLookAtWeight(0.3f, 0f, 0.3f);
                }
            }
            else
            {
                if (isAiming)
                {
                    animator.SetLookAtWeight(1f, 0f, 1f);
                }
                else
                {
                    animator.SetLookAtWeight(0.3f, 0.3f, 0.3f);
                } 
            }
            
            animator.SetLookAtPosition(targetLook.position);
            
            if (leftHandTarget != null)
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
            leftHandTarget = currentWeapon.magHandTarget;
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
            leftHandTarget = currentWeapon.leftHandTarget;
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
        private CarController _car;
        public void EnterInCar(CarController car, Action onEnteringAnimFinished)
        {
            _car = car;
            _onEnteringAnimFinished = onEnteringAnimFinished;
            animator.SetFloat(id_horizontal,0);
            animator.SetFloat(id_vertical,0);
            animator.SetTrigger(id_enteringCar);
        }

        public void StartGrabbingCarDoorToOpen()
        {
            var clips = animator.GetCurrentAnimatorClipInfo(0);
            float ikDuration = clips[0].clip.events[1].time - clips[0].clip.events[0].time;
            
            leftHandTarget = _car.DriverDoorHandleTarget;
            SetLeftHandWeight(1, ikDuration);
        }
        public void GrabCarDoorToOpen()
        {
            _car.OpenDoor();
        }
        
        public void UnGrabCarDoorToOpen()
        {
            leftHandTarget = null;
            SetLeftHandWeight(0);
        }
        
        public void GrabCarDoorToClose()
        {
            
        }
        
        public void UnGrabCarDoorToClose()
        {
            
        }

        public void FinallyEnterCar()
        {
            _onEnteringAnimFinished?.Invoke();
            _onEnteringAnimFinished = null;
            _car = null;
        }
        

        public void ExitFromCar()
        {
            //animator.SetBool(id_sitingInCar,false);
        }
        

        #endregion
    }
}
