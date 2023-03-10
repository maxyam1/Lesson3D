using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Character.Scripts
{
    public class PlayerAnimations : AbstractCharacterAnimations
    {

        protected override void StandardLocomotion(float horizontal, float vertical)
        {
            //Передвижение
            float moveAmount = Mathf.Clamp01(new Vector2(horizontal, vertical).magnitude);
            animator.SetFloat("vertical", moveAmount);
            
            //Вращение
            Vector3 moveDirection = targetLook.forward * vertical + targetLook.right * horizontal;
            moveDirection.y = 0;//чтобы не наклоняться по вертикали

            if (moveDirection == Vector3.zero)//Если никуда не идем, то смотрим куда смотрели
            {
                moveDirection = transform.forward;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(moveDirection),Time.deltaTime * rotationSpeed);//Slerp для плавного поворота персонажа
        }

        protected override void AimingLocomotion(float horizontal, float vertical)
        {
            //Передвижение
            animator.SetFloat("horizontal", horizontal);
            animator.SetFloat("vertical", vertical);
            
            //Вращение
            Vector3 moveDirection = targetLook.forward;
            moveDirection.y = 0;//чтобы не наклоняться по вертикали

            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(moveDirection),Time.deltaTime * rotationSpeed);//Slerp для плавного поворота персонажа
        }
    }
}
