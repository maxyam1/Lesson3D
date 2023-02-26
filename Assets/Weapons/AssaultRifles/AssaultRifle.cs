using System;
using UnityEngine;

namespace Weapons.AssaultRifles
{
    public class AssaultRifle : Weapon
    {
        [SerializeField] protected float shootsPerMin;

        protected void Update()
        {
            Debug.DrawLine(shotPoint.position, shotPoint.position + shotPoint.forward * 100, Color.blue);
        }

        protected override void Shoot()
        {
            base.Shoot();
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
