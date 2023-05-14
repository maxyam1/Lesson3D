using UnityEngine;

namespace Weapons.Pistols
{
    public class Pistol : Weapon
    {

        protected void Update()
        {
            Debug.DrawLine(shotPoint.position, shotPoint.position + shotPoint.forward * 100, Color.blue);
        }

        protected override bool Shoot()
        {
            if (!base.Shoot())
                return false;

            return true;
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
