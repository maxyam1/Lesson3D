using UnityEngine;

namespace Character.Scripts
{
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        
        public void SetAiming(bool isAiming)
        {
            animator.SetBool("isAiming", isAiming);
        }

        private void StandardLocomotion()
        {
            
        }

        private void AimingLocomotion(float horizontal, float vertical)
        {
            
        }

        public void Locomotion(float horizontal, float vertical)
        {
            animator.SetFloat("horizontal", horizontal);
            animator.SetFloat("vertical", vertical);
        }
    }
}
