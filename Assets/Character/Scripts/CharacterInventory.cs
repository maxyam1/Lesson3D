using System;
using UnityEngine;
using Weapons;

namespace Character.Scripts
{
    public class CharacterInventory : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform rightHand;
        
        [SerializeField] private Weapon firstWeapon;
        [SerializeField] private Weapon secondWeapon;

        public Weapon currentWeapon;

        private void Start()
        {
            characterController.characterAnimations.WeaponChanged(currentWeapon);
        }

        public void ChangeWeapon(int i)
        {
            if (currentWeapon != null)
            {
                Destroy(currentWeapon.gameObject);
            }
            
            Weapon weaponForSpawn = null;
            if (i == 1)
            {
                weaponForSpawn = firstWeapon;
            }else if (i == 2)
            {
                weaponForSpawn = secondWeapon;
            }

            if (weaponForSpawn != null)
            {
                currentWeapon = Instantiate(weaponForSpawn, rightHand);
                currentWeapon.transform.localRotation = Quaternion.identity;
                currentWeapon.transform.localPosition = Vector3.zero;
                characterController.characterAnimations.WeaponChanged(currentWeapon);   
            }
        }
    }
}
