using UnityEngine;

namespace Camera
{
    [CreateAssetMenu(menuName = "Camera/Camera Controller Settings")]
    public class CameraSettings : ScriptableObject
    {
        public float aimingSpeed;
        public float rotationSpeedY;
        public float rotationSpeedX;
        public float minAngle;
        public float maxAngle;
        public Vector3 pivotPos;
        public float cameraDistanceFromPivot;
        public Vector3 pivotPosAiming;
        public float cameraDistanceFromPivotAiming;
    }
}
