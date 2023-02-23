using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public Transform leftHandTarget;
        public Vector3 rotationPivotPos;
        public Vector3 rightHandPos;
        public Vector3 rightHandRot;
    }
}
