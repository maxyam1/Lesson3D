using UnityEngine;

namespace Vehicles
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] protected WheelCollider wheelCollider_FL;
        [SerializeField] protected Transform wheelRenderer_FL;
        
        [SerializeField] protected WheelCollider wheelCollider_FR;
        [SerializeField] protected Transform wheelRenderer_FR;
        
        [SerializeField] protected WheelCollider wheelCollider_RL;
        [SerializeField] protected Transform wheelRenderer_RL;
        
        [SerializeField] protected WheelCollider wheelCollider_RR;
        [SerializeField] protected Transform wheelRenderer_RR;

        private Vector3 tmpPos;
        private Quaternion tmpRot;
        void Start()
        {
        
        }
        
        void Update()
        {
            wheelCollider_FL.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_FL.position = tmpPos;
            wheelRenderer_FL.rotation = tmpRot;
           
            wheelCollider_FR.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_FR.position = tmpPos;
            wheelRenderer_FR.rotation = tmpRot;
            
            wheelCollider_RL.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_RL.position = tmpPos;
            wheelRenderer_RL.rotation = tmpRot;
            
            wheelCollider_RR.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_RR.position = tmpPos;
            wheelRenderer_RR.rotation = tmpRot;
        }
    }
}
