using System;
using Character.Scripts;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(AudioSource))]
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
        [SerializeField] protected float scareNpcRadius;
        [SerializeField] protected LayerMask characterCapsuleMask;
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected AudioClip shotSound;
        
        [SerializeField] protected int maxBulletsInMagazine;
        protected int currentBulletsCountInMagazine;

        public WeaponOnGround weaponOnGroundPrefab;

        protected virtual void Start()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }

            currentBulletsCountInMagazine = maxBulletsInMagazine;
        }

        public virtual void Reload()
        {
            currentBulletsCountInMagazine = maxBulletsInMagazine;
        }

        protected virtual bool Shoot()
        {
            if(currentBulletsCountInMagazine <= 0)
                return false;

            muzzleFlash.Play();
            cartridgeEjectEffect.Play();
            audioSource.PlayOneShot(shotSound);
            Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation).damage = bulletDamage;
            ScareNpcByShot();
            currentBulletsCountInMagazine--;
            
            return true;
        }

        public abstract void TriggerPressed();

        public abstract void TriggerUnPressed();

        protected void ScareNpcByShot()//Todo переделать через реестр npc
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
