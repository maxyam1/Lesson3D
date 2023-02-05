using System;
using UnityEngine;

namespace Character.Scripts
{
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform camera;
        [SerializeField] private float rotationSpeed = 10;
        private bool _isAiming;
        
        public void SetAiming(bool isAiming)
        {
            _isAiming = isAiming;
            animator.SetBool("isAiming", isAiming);
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
    }
}
