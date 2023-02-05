using System;
using Camera;
using UnityEngine;

namespace Character.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterAnimations characterAnimations;
        [SerializeField] private CameraController cameraController;
        
        void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                ChangeAiming(false);
            }else if (Input.GetMouseButtonDown(1))
            {
                ChangeAiming(true);
            }
            

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            characterAnimations.Locomotion(horizontal, vertical);
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
    }
}
