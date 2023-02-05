using System;
using Camera;
using UnityEngine;

namespace Character.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterAnimations characterAnimations;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private LayerMask notPlayerCapsuleCollider;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float jumpForce = 1;
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
        }

        public void Jump()
        {
            rigidbody.AddForce(Vector3.up * jumpForce,ForceMode.VelocityChange);
        }
    }
}
