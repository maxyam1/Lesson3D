using UnityEngine;

namespace Character.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterAnimations characterAnimations;

        void Update()
        {
            characterAnimations.SetAiming(Input.GetMouseButton(1));

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            characterAnimations.Locomotion(horizontal, vertical);
        }
    }
}
