using UnityEngine;

namespace Weapons
{
    public class WeaponOnGround : MonoBehaviour, IUsable
    {
        public Weapon weaponPrefab;
        public void Use(CharacterController user)
        {
        }

        public UsableType GetUsableType()
        {
            return UsableType.WeaponOnGround;
        }
    }
}
