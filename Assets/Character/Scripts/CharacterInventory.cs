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
        private WeaponSlot _currentWeaponId;

        [SerializeField] protected SerializableDictionary<BulletType, int> bulletCounts = new SerializableDictionary<BulletType, int>();

        private (WeaponSlot from, WeaponSlot to)? _currentWeaponChanging = null;


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

        #region ==ChangingWeapon==

        private Action _onWeaponChangedCallback;
        public bool ChangeWeapon(WeaponSlot slot, Action callback = null)
        {
            if (_currentWeaponId == slot || (currentWeapon == null && weaponSlots[(int)slot] == null) || _currentWeaponChanging.HasValue)
            {
                return false;
            }

            _onWeaponChangedCallback = callback;
            _currentWeaponChanging = (_currentWeaponId, slot);
            //_weaponForSpawn = weaponSlots[(int)slot];
            _currentWeaponId = slot;
            

            if (currentWeapon != null)
            {
                characterCharacterController.characterAnimations.PutGun();
            }
            else if(weaponSlots[(int)slot])
            {
                characterCharacterController.characterAnimations.TakeGun();
            }

            return true;
        }

        public void ChangeWeapon(WeaponOnGround weaponOnGround)//todo допилить выбрасывание
        {
            if (_currentWeaponId == WeaponSlot.NoWeapon || _currentWeaponChanging.HasValue)
            {
                return;
            }

            Weapon weaponToDrop = weaponSlots[(int)_currentWeaponId];
            weaponSlots[(int)_currentWeaponId] = weaponOnGround.weaponPrefab;
            WeaponSlot currentID = _currentWeaponId;
            _currentWeaponId = (WeaponSlot)(-1);
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
            if (weaponSlots[(int)_currentWeaponChanging.Value.to])
            {
                characterCharacterController.characterAnimations.TakeGun();
            }
            else
            {
                _currentWeaponChanging = null;
                try
                {
                    _onWeaponChangedCallback?.Invoke();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                _onWeaponChangedCallback = null;
            }
        }
        
        public void WeaponTaken()
        {
            if (weaponSlots[(int)_currentWeaponChanging.Value.to])
            {
                currentWeapon = Instantiate(weaponSlots[(int)_currentWeaponChanging.Value.to], rightHand);
                currentWeapon.transform.localRotation = Quaternion.identity;
                currentWeapon.transform.localPosition = Vector3.zero;
                //characterCharacterController.characterAnimations.WeaponChanged(currentWeapon);
                OnCurrentWeaponChanged?.Invoke(currentWeapon);
                _currentWeaponChanging = null;
            }
            try
            {
                _onWeaponChangedCallback?.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            _onWeaponChangedCallback = null;
        }

        #endregion

        public bool CanReload()
        {
            if (!currentWeapon)
                return false;

            if (!bulletCounts.ContainsKey(currentWeapon.BulletType))
                return false;
            
            return bulletCounts[currentWeapon.BulletType] > 0 && currentWeapon.LacksBullet > 0;
        }
    }

    public enum WeaponSlot
    {
        MainWeapon = 0,
        SecondaryWeapon = 1,
        NoWeapon = 2
    }
}
