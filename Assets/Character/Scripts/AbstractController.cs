using System.Collections.Generic;
using UnityEngine;

namespace Character.Scripts
{
    public abstract class AbstractController : MonoBehaviour
    {
        public CharacterInventory inventory;
        public CharacterAnimations characterAnimations;
        [SerializeField] protected float _hp = 100;
        [SerializeField] protected Rigidbody rigidbody;
        [SerializeField] protected List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();


        private void Start()
        {
            foreach (var rb in ragdollRigidbodies)
            {
                rb.isKinematic = true;
            }
        }

        public void TakeDamage(float damage)
        {
            _hp -= damage;
            
            if (_hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            foreach (var rb in ragdollRigidbodies)
            {
                rb.isKinematic = false;
            }
            
            Destroy(this);
            Destroy(GetComponent<Animator>());
            Destroy(rigidbody);
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(gameObject, 60);
        }
    }
}
