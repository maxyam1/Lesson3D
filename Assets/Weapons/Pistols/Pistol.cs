using UnityEngine;

namespace Weapons.Pistols
{
    public class Pistol : Weapon
    {

        protected void Update()
        {
            Debug.DrawLine(shotPoint.position, shotPoint.position + shotPoint.forward * 100, Color.blue);
        }

        protected override void Shoot()
        {
            muzzleFlash.Play();
            cartridgeEjectEffect.Play();
            Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation).damage = bulletDamage;
        }

        public override void TriggerPressed()
        {
            Shoot();
        }

        public override void TriggerUnPressed()
        {
            
        }
    }
}
