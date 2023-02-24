using UnityEngine;

namespace Character.Scripts
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);
    }

    public class BodyPart : MonoBehaviour, IDamageable
    {
        [SerializeField] private AbstractController controller;
        [SerializeField] private float damageFactor = 1;


        public void TakeDamage(float damage)
        {
            if (controller != null)
            {
                controller.TakeDamage(damage * damageFactor);   
            }
        }
    }
}
