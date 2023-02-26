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
