using System;
using UnityEngine;
using Weapons;

namespace Character.Scripts
{
    public class CharacterInventory : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        
        private Weapon _firstWeapon;
        private Weapon _secondWeapon;

        public Weapon currentWeapon;

        private void Start()
        {
            characterController.characterAnimations.WeaponChanged(currentWeapon);
        }
    }
}
