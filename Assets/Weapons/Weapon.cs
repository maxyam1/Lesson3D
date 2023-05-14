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

        public GameObject magPrefab;
        public GameObject magOnWeapon;
        public Transform magHandTarget;
        protected bool isReloading;
        
        [SerializeField] protected int maxBulletsInMagazine;
        public int currentBulletsCountInMagazine;

        public WeaponOnGround weaponOnGroundPrefab;

        public int LacksBullet
        {
            get
            {
                return maxBulletsInMagazine - currentBulletsCountInMagazine;
            }
        }

        public abstract BulletType BulletType
        {
            get;
        }

        protected virtual void Start()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }

            currentBulletsCountInMagazine = maxBulletsInMagazine;
        }

        public void StartReload()
        {
            isReloading = true;
        }

        protected virtual bool Shoot()
        {
            if(currentBulletsCountInMagazine <= 0 && !isReloading)
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

        public void BoltPulled()
        {
            //UpdateBulletsUi(); //TODO
        }

        public void ReloadFinished()
        {
            isReloading = false;
        }
    }

    public enum BulletType
    {
        Rifle,
        Pistol,
        Rpg,
        ShotGun
    }
}
