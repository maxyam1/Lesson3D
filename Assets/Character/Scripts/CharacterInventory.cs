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
        private int _currentWeaponId;

        private Weapon _weaponForSpawn;
        

        public void ChangeWeapon(int i)
        {
            if (_currentWeaponId == i)
            {
                return;
            }

            _weaponForSpawn = null;
            if (i == 0)
            {
                _currentWeaponId = 0;
            }
            if (i == 1)
            {
                _weaponForSpawn = firstWeapon;
                _currentWeaponId = 1;
            }else if (i == 2)
            {
                _weaponForSpawn = secondWeapon;
                _currentWeaponId = 2;
            }

            if (!_weaponForSpawn)
            {
                _currentWeaponId = 0;
            }


            if (currentWeapon != null)
            {
                characterController.characterAnimations.PutGun();
            }
            else if(_weaponForSpawn)
            {
                characterController.characterAnimations.TakeGun();
            }
        }

        public void WeaponPutted()
        {
            Destroy(currentWeapon.gameObject);
            if (_weaponForSpawn)
            {
                characterController.characterAnimations.TakeGun();
            }
        }
        
        public void WeaponTaken()
        {
            if (_weaponForSpawn != null)
            {
                currentWeapon = Instantiate(_weaponForSpawn, rightHand);
                currentWeapon.transform.localRotation = Quaternion.identity;
                currentWeapon.transform.localPosition = Vector3.zero;
                characterController.characterAnimations.WeaponChanged(currentWeapon);   
                _weaponForSpawn = null;
            }
        }
    }
}
