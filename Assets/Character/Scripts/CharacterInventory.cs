using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Character.Scripts
{
    public class CharacterInventory : MonoBehaviour
    {
        [FormerlySerializedAs("characterController")] [SerializeField] private AbstractCharacterController characterCharacterController;
        [SerializeField] private Transform rightHand;
        
        [SerializeField] private Weapon[] weaponSlots = new Weapon[3];

        public Weapon currentWeapon;
        private int _currentWeaponId;

        private Weapon _weaponForSpawn;
        

        public void ChangeWeapon(int i)
        {
            if (_currentWeaponId == i || (currentWeapon == null && weaponSlots[i] == null))
            {
                return;
            }

            _weaponForSpawn = weaponSlots[i];
            _currentWeaponId = i;
            

            if (currentWeapon != null)
            {
                characterCharacterController.characterAnimations.PutGun();
            }
            else if(_weaponForSpawn)
            {
                characterCharacterController.characterAnimations.TakeGun();
            }
        }

        public void ChangeWeapon(WeaponOnGround weaponOnGround)
        {
            if (_currentWeaponId == 2)
            {
                return;
            }

            Weapon weaponToDrop = weaponSlots[_currentWeaponId];
            weaponSlots[_currentWeaponId] = weaponOnGround.weaponPrefab;
            int currentID = _currentWeaponId;
            _currentWeaponId = -1;
            ChangeWeapon(currentID);
            if (weaponToDrop)
            {
                Instantiate(weaponToDrop.weaponOnGroundPrefab, weaponOnGround.transform.position,
                    weaponOnGround.transform.rotation);   
            }
            Destroy(weaponOnGround.gameObject);
        }

        public void WeaponPutted()
        {
            Destroy(currentWeapon.gameObject);
            if (_weaponForSpawn)
            {
                characterCharacterController.characterAnimations.TakeGun();
            }
        }
        
        public void WeaponTaken()
        {
            if (_weaponForSpawn != null)
            {
                currentWeapon = Instantiate(_weaponForSpawn, rightHand);
                currentWeapon.transform.localRotation = Quaternion.identity;
                currentWeapon.transform.localPosition = Vector3.zero;
                characterCharacterController.characterAnimations.WeaponChanged(currentWeapon);   
                _weaponForSpawn = null;
            }
        }
    }
}
