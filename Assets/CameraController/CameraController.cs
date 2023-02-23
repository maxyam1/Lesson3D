using System;
using UnityEngine;

namespace CameraController
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform pivotTransform;
        [SerializeField] private Transform followTarget;
        [SerializeField] private Transform targetLook;
        [SerializeField] private LayerMask targetLookRayMask;
        [SerializeField] private CameraSettings cameraSettings;

        public bool isAiming;
        private float _verticalAngle;

        public void UpdateCamera(float mouseX,float mouseY)
        {
            UpdatePosition();
            UpdateRotation(mouseX, mouseY);
            UpdateTargetLook();
        }

        private void UpdateTargetLook()
        {
            RaycastHit hit;
            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 200,targetLookRayMask);

            float distance = 200;
            
            if (hit.collider != null)
            {
                distance = hit.distance;
            }

            targetLook.position = cameraTransform.position + cameraTransform.forward * distance;
        }

        private void UpdatePosition()
        {
            transform.position = followTarget.position;

            Vector3 pivotTargetPosition;
            Vector3 cameraTargetPosition;
            
            if (isAiming)
            {
                pivotTargetPosition = cameraSettings.pivotPosAiming;
                cameraTargetPosition = Vector3.back * cameraSettings.cameraDistanceFromPivotAiming;
            }
            else
            {
                pivotTargetPosition = cameraSettings.pivotPos;
                cameraTargetPosition = Vector3.back * cameraSettings.cameraDistanceFromPivot;
            }

            float t = Time.deltaTime * cameraSettings.aimingSpeed;
            pivotTransform.localPosition = Vector3.Lerp(pivotTransform.localPosition, pivotTargetPosition, t);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraTargetPosition, t);
        }

        private void UpdateRotation(float mouseX,float mouseY)
        {
            //Вращение по горизонтали
            transform.rotation *= Quaternion.Euler(0,mouseX * cameraSettings.rotationSpeedX,0);

            //Вращение по вертикали 
            _verticalAngle -= mouseY * cameraSettings.rotationSpeedY;
            
            _verticalAngle = Mathf.Clamp(_verticalAngle, cameraSettings.minAngle, cameraSettings.maxAngle);
            pivotTransform.localRotation = Quaternion.Euler(_verticalAngle,0,0);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(targetLook.position, 0.2f);
        }
    }
}
