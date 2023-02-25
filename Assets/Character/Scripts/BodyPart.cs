using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Scripts
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);
    }

    public class BodyPart : MonoBehaviour, IDamageable
    {
        [FormerlySerializedAs("controller")] [SerializeField] private AbstractCharacterController characterController;
        [SerializeField] private float damageFactor = 1;


        public void TakeDamage(float damage)
        {
            if (characterController != null)
            {
                characterController.TakeDamage(damage * damageFactor);   
            }
        }
    }
}
