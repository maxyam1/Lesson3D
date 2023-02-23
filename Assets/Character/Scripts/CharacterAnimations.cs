using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Character.Scripts
{
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform camera;
        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] private Transform targetLook;

        private bool _isAiming;
        private bool _isGrounded;

        private Transform _weaponRotationPivot;
        private Transform _leftHandTarget;
        private Transform _rightHandTarget;
        private float _rightHandWeight;
        
        private Vector3 _leftHandTargetPos;
        private Quaternion _leftHandTargetRot;
        private Vector3 _rightHandTargetPos;
        private Quaternion _rightHandTargetRot;

        private void Awake()
        {
            if (_weaponRotationPivot == null)
            {
                _weaponRotationPivot = new GameObject().transform;
                _weaponRotationPivot.name = "weapon rotation pivot";
                _weaponRotationPivot.parent = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            }

            if (_rightHandTarget == null)
            {
                _rightHandTarget = new GameObject().transform;
                _rightHandTarget.name = "right hand target";
                _rightHandTarget.parent = _weaponRotationPivot;   
            }
        }

        public void WeaponChanged(Weapon currentWeapon)
        {
            if (currentWeapon == null)
            {
                _leftHandTarget = null;
            }
            else
            {
                _weaponRotationPivot.localPosition = currentWeapon.rotationPivotPos;
                _leftHandTarget = currentWeapon.leftHandTarget;
                _rightHandTarget.localPosition = currentWeapon.rightHandPos;
                _rightHandTarget.localRotation = Quaternion.Euler(currentWeapon.rightHandRot);   
            }
        }

        public void SetAiming(bool isAiming)
        {
            _isAiming = isAiming;
            animator.SetBool("isAiming", isAiming);

            if (isAiming)
            {
                DOTween.To(()=>_rightHandWeight, (value)=>_rightHandWeight = value, 1, 0.5f);
            }
            else
            {
                DOTween.To(()=>_rightHandWeight, (value)=>_rightHandWeight = value, 0, 0.5f);
            }
        }

        private void StandardLocomotion(float horizontal, float vertical)
        {
            //Передвижение
            float moveAmount = Mathf.Clamp01(new Vector2(horizontal, vertical).magnitude);
            animator.SetFloat("vertical", moveAmount);
            
            //Вращение
            Vector3 moveDirection = camera.forward * vertical + camera.right * horizontal;
            moveDirection.y = 0;//чтобы не наклоняться по вертикали

            if (moveDirection == Vector3.zero)//Если ни куда не идем, то смотрим куда смотрели
            {
                moveDirection = transform.forward;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(moveDirection),Time.deltaTime * rotationSpeed);//Slerp для плавного поворота персонажа
        }

        private void AimingLocomotion(float horizontal, float vertical)
        {
            //Передвижение
            animator.SetFloat("horizontal", horizontal);
            animator.SetFloat("vertical", vertical);
            
            //Вращение
            Vector3 moveDirection = camera.forward;
            moveDirection.y = 0;//чтобы не наклоняться по вертикали

            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(moveDirection),Time.deltaTime * rotationSpeed);//Slerp для плавного поворота персонажа
        }

        public void Locomotion(float horizontal, float vertical)
        {
            if (_isAiming)
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
            if (!_isGrounded && isGrounded)
            {
                _isGrounded = true;
                animator.SetBool("isGrounded",true);
            }
            else if(_isGrounded && !isGrounded)
            {
                _isGrounded = false;
                animator.SetBool("isGrounded",false);
            }
        }

        public void SetJump()
        {
            animator.SetTrigger("jump");
        }


        private void Update()
        {
            // Если делать это напрямую у рук появляется тряска и не точность
            _leftHandTargetPos = _leftHandTarget.position;
            _leftHandTargetRot = _leftHandTarget.rotation;

            _rightHandTargetPos = _rightHandTarget.position;
            _rightHandTargetRot = _rightHandTarget.rotation;
        }

        //IK
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (_isAiming)
            {
                animator.SetLookAtWeight(1f, 0f, 1f);
            }
            else
            {
                animator.SetLookAtWeight(0.3f, 0.3f, 0.3f);
            }
            
            animator.SetLookAtPosition(targetLook.position);
            
            if (_leftHandTarget != null)
            {
                _weaponRotationPivot.LookAt(targetLook);
                
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTargetPos);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTargetRot);   
                
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightHandWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTargetPos);
                animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTargetRot);
            }
        }
    }
}
