using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.SerializableDict;
using Weapons;

namespace Character.Scripts
{
    public class CharacterInventory : MonoBehaviour
    {
        public event Action<Weapon> OnCurrentWeaponChanged; 
        
        [FormerlySerializedAs("characterController")] [SerializeField] private AbstractCharacterController characterCharacterController;
        [SerializeField] private Transform rightHand;
        
        [SerializeField] private Weapon[] weaponSlots = new Weapon[3];
        
        public Weapon currentWeapon;
        private int _currentWeaponId;

        private Weapon _weaponForSpawn;

        [SerializeField] protected SerializableDictionary<BulletType, int> bulletCounts = new SerializableDictionary<BulletType, int>();


        public void GetBulletsToMag()
        {
            BulletType bulletType = currentWeapon.BulletType;
            int targetBullets = currentWeapon.LacksBullet;

            if (!bulletCounts.ContainsKey(bulletType))
            {
                return;
            }

            if (bulletCounts[bulletType] >= targetBullets)
            {
                bulletCounts[bulletType] -= targetBullets;
                currentWeapon.currentBulletsCountInMagazine += targetBullets;
                return;
            }

            currentWeapon.currentBulletsCountInMagazine += bulletCounts[bulletType];
            bulletCounts[bulletType] = 0;
        }

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
                //characterCharacterController.characterAnimations.WeaponChanged(currentWeapon);
                OnCurrentWeaponChanged?.Invoke(currentWeapon);
                _weaponForSpawn = null;
            }
        }

        public bool CanReload()
        {
            if (!currentWeapon)
                return false;

            if (!bulletCounts.ContainsKey(currentWeapon.BulletType))
                return false;
            
            return bulletCounts[currentWeapon.BulletType] > 0 && currentWeapon.LacksBullet > 0;
        }
    }
}
