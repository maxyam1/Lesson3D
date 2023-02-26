using UnityEngine;

namespace Character.Scripts.Npc
{
    public class NpcAnimations : AbstractCharacterAnimations
    {
        public void StandardLocomotion(Vector3 targetPos, float speed)
        {
            Vector3 direction = targetPos - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction),Time.deltaTime * rotationSpeed);//Slerp для плавного поворота персонажа
            StandardLocomotion(0, speed);
        }

        protected override void StandardLocomotion(float horizontal, float vertical)
        {
            //Передвижение
            animator.SetFloat("vertical", vertical);
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
