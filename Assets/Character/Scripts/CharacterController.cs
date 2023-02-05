using System;
using Camera;
using UnityEngine;

namespace Character.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterAnimations characterAnimations;
        [SerializeField] private CameraController cameraController;
        
        private bool _isAiming;
        
        void Update()
        {
            bool isAiming = Input.GetMouseButton(1);
            if (!isAiming && _isAiming)
            {
                _isAiming = false;
                ChangeAiming(false);
            }else if (isAiming && !_isAiming)
            {
                _isAiming = true;
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
