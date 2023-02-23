using UnityEngine;

namespace Weapons.AssaultRifles
{
    public class AssaultRifle : Weapon
    {
        [SerializeField] protected Transform TargetLook;//TEMP
        protected void Update()
        {
            Debug.DrawLine(shotPoint.position, shotPoint.position + shotPoint.forward * 100, Color.blue);
        }
    }
}
