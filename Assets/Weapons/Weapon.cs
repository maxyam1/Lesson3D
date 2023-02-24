using System;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public Transform leftHandTarget;
        public Vector3 rotationPivotPos;
        public Vector3 rightHandPos;
        public Vector3 rightHandRot;
        [SerializeField] protected Transform shotPoint;
        [SerializeField] protected ParticleSystem muzzleFlash;
        [SerializeField] protected ParticleSystem cartridgeEjectEffect;
        [SerializeField] protected Bullet bulletPrefab;
        [SerializeField] protected float bulletDamage;
        [SerializeField] protected float maxBulletsInMagazine;

        public WeaponOnGround weaponOnGroundPrefab; 

        protected abstract void Shoot();

        public abstract void TriggerPressed();

        public abstract void TriggerUnPressed();
    }
}
