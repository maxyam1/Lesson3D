using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Character.Scripts
{
    public abstract class AbstractCharacterAnimations : MonoBehaviour
    {
        [SerializeField] protected AbstractCharacterController characterController;
        [SerializeField] protected Animator animator;
        [SerializeField] protected float rotationSpeed = 10;
        [SerializeField] protected Transform targetLook;
        
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

        private void Awake()
        {
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

        public void WeaponChanged(Weapon currentWeapon)
        {
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

        public void SetAiming(bool isAiming)
        {
            this.isAiming = isAiming;
            animator.SetBool("isAiming", isAiming);

            if (isAiming)
            {
                DOTween.To(()=>leftHandWeight, (value)=>leftHandWeight = value, 1, 0.5f);
                DOTween.To(()=>rightHandWeight, (value)=>rightHandWeight = value, 1, 0.5f);
            }
            else
            {
                DOTween.To(()=>leftHandWeight, (value)=>leftHandWeight = value, 0, 0.5f);
                DOTween.To(()=>rightHandWeight, (value)=>rightHandWeight = value, 0, 0.5f);

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
                animator.SetBool("isGrounded",true);
            }
            else if(this.isGrounded && !isGrounded)
            {
                this.isGrounded = false;
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
            animator.SetTrigger("putGun");
        }

        public void TakeGun()
        {
            animator.SetTrigger("takeGun");

        }

        //IK
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (targetLook == null)
            {
                return;
            }

            if (isAiming)
            {
                animator.SetLookAtWeight(1f, 0f, 1f);
            }
            else
            {
                animator.SetLookAtWeight(0.3f, 0.3f, 0.3f);
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
    }
}
