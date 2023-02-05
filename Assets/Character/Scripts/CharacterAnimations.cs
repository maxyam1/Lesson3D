using UnityEngine;

namespace Character.Scripts
{
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private bool _isAiming;
        
        public void SetAiming(bool isAiming)
        {
            if (!isAiming && _isAiming)
            {
                _isAiming = false;
                animator.SetBool("isAiming", _isAiming);
            }else if (isAiming && !_isAiming)
            {
                _isAiming = true;
                animator.SetBool("isAiming", _isAiming);
            }
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
