using System;
using Character.Scripts;
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
        [SerializeField] protected float scareNpcRadius;
        [SerializeField] protected LayerMask characterCapsuleMask;

        public WeaponOnGround weaponOnGroundPrefab;

        protected virtual void Shoot()
        {
            muzzleFlash.Play();
            cartridgeEjectEffect.Play();
            Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation).damage = bulletDamage;
            ScareNpcByShot();
        }

        public abstract void TriggerPressed();

        public abstract void TriggerUnPressed();

        protected void ScareNpcByShot()
        {
            Collider[] objects = Physics.OverlapSphere(transform.position, scareNpcRadius, characterCapsuleMask);

            foreach (var obj in objects)
            {
                IScareableByShot scareableByShot = obj.GetComponent<IScareableByShot>();
                if (scareableByShot != null)
                {
                    scareableByShot.Scare(transform.position);
                }
            }
        }
    }
}
