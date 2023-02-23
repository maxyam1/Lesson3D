using System;
using UnityEngine;

namespace Character.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        public CharacterAnimations characterAnimations;
        public CharacterInventory inventory;
        [SerializeField] private CameraController.CameraController cameraController;
        [SerializeField] private LayerMask notPlayerCapsuleCollider;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float jumpForce = 1;

        private bool _isAiming;

        void Update()
        {
            //Прицеливание
            if (Input.GetMouseButtonUp(1))
            {
                ChangeAiming(false);
            }else if (Input.GetMouseButtonDown(1))
            {
                ChangeAiming(true);
            }
            
            //Прыжок
            bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f,Vector3.down,0.3f,notPlayerCapsuleCollider);
            if (isGrounded)
            {
                Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.down * 0.2f, Color.red);//Рисуем луч на сцене, чтобы видеть его
                
                //Передвижение
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
            
                characterAnimations.Locomotion(horizontal, vertical);

                if (_isAiming && Input.GetMouseButtonDown(0))
                {
                    inventory.currentWeapon.TriggerPressed();
                }

                if (_isAiming || Input.GetMouseButtonUp(0))
                {
                    inventory.currentWeapon.TriggerUnPressed();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    characterAnimations.SetJump();
                }
            }
            else
            {
                Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.down * 0.2f, Color.blue);//Снова рисуем луч, только другого цвета, он никуда не попал
            }
            
            characterAnimations.SetIsGrounded(isGrounded);
        }

        private void LateUpdate()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            cameraController.UpdateCamera(mouseX, mouseY);
        }

        private void ChangeAiming(bool isAiming)
        {
            characterAnimations.SetAiming(isAiming);
            cameraController.isAiming = isAiming;
            _isAiming = isAiming;
        }

        public void Jump()
        {
            rigidbody.AddForce(Vector3.up * jumpForce,ForceMode.VelocityChange);
        }
    }
}
