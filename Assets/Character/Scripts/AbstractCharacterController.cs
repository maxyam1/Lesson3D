using System;
using System.Collections.Generic;
using Ui;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Vehicles;
using Weapons;

namespace Character.Scripts
{
    public abstract class AbstractCharacterController : MonoBehaviour
    {
        protected CarController car;
        
        public CharacterInventory inventory;
        public AbstractCharacterAnimations characterAnimations;
        [SerializeField] protected int ragdollLayerInCar;
        [SerializeField] protected int ragdollLayer;
        [SerializeField] protected float _hp = 100;
        [SerializeField] protected Rigidbody rigidbody;
        [SerializeField] protected List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
        [FormerlySerializedAs("capsuleCollide")] [SerializeField] protected CapsuleCollider capsuleCollider;

        private RigidBodySave _savedRigidbody;

        public bool InCar
        {
            get { return car; }
        }

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
        
        protected void EnterInCar(CarController car)
        {
            Action onWeaponTakedAway = () =>
            {
                capsuleCollider.enabled = false;
                SetLayerToRagdoll(ragdollLayerInCar);
                SaveRigidBody();

                characterAnimations.StartCoroutine(characterAnimations.EnterInCar(car, () =>
                {
                    this.car = car;
                    transform.SetParent(car.Seat);
                    transform.localRotation = Quaternion.identity;
                    CarUI.Car = car;
                    car.TurnOnOffEngine(true);
                    transform.localPosition = Vector3.zero;
                }));
            };

            if (!inventory.ChangeWeapon(WeaponSlot.NoWeapon, onWeaponTakedAway))
            {
                onWeaponTakedAway.Invoke();
            }
        }
        protected void ExitFromCar()
        {
            CarUI.Car = null;
            car.TurnOnOffEngine(false);
            SetLayerToRagdoll(ragdollLayer);
            characterAnimations.GetOutFromCar();
            LoadRigidBody();
            capsuleCollider.enabled = true;
            car = null;
        }
        
        private void SaveRigidBody()
        {
            RigidBodySave.ToStruct(rigidbody, out _savedRigidbody);
            Destroy(rigidbody);
            rigidbody = null;
        }

        private void LoadRigidBody()
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            RigidBodySave.FromStruct(rigidbody, _savedRigidbody);
        }

        protected void SetLayerToRagdoll(int layer)
        {
            foreach (var bodyPart in ragdollRigidbodies)
            {
                bodyPart.gameObject.layer = layer;
            }
        }
    }
    
}
