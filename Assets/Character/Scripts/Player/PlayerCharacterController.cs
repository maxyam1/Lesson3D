using System;
using UnityEngine;
using Vehicles;
using Weapons;

namespace Character.Scripts
{
    public class PlayerCharacterController : AbstractCharacterController
    {
        [SerializeField] private CameraController.CameraController cameraController;
        [SerializeField] private LayerMask notPlayerCapsuleCollider;
        [SerializeField] private float jumpForce = 1;

        private bool _isAiming;

        void Update()
        {
            if (car)
            {
                CarMovement();
            }
            else
            {
                StandardMovement();
            }
        }

        private void StandardMovement()
        {
            //Прицеливание
            if (Input.GetMouseButtonUp(1))
            {
                ChangeAiming(false);
            }
            if (Input.GetMouseButtonDown(1) && inventory.currentWeapon != null)
            {
                ChangeAiming(true);
            }
            
            //Прыжок
            bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f,Vector3.down,0.3f,notPlayerCapsuleCollider);
            if (isGrounded)
            {
                Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.down * 0.2f, Color.red);//Рисуем луч на сцене, чтобы видеть его
                
                //Передвижение
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
            
                characterAnimations.Locomotion(horizontal, vertical);

                //Смена оружия
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    inventory.ChangeWeapon(WeaponSlot.MainWeapon);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    inventory.ChangeWeapon(WeaponSlot.SecondaryWeapon);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    inventory.ChangeWeapon(WeaponSlot.NoWeapon);
                }
                
                //Стрельба
                if (_isAiming && Input.GetMouseButtonDown(0))
                {
                    inventory.currentWeapon.TriggerPressed();
                }

                if (!_isAiming || Input.GetMouseButtonUp(0))
                {
                    inventory.currentWeapon?.TriggerUnPressed();
                }
                
                //Перезарядка
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (inventory.CanReload())
                    {
                        inventory.GetBulletsToMag();
                        characterAnimations.Reload();   
                    }
                }

                //Прыжок
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    characterAnimations.SetJump();
                }
                
                //Взаимодействие
                if(Input.GetKeyDown(KeyCode.E))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(cameraController.cameraTransform.position,
                            cameraController.cameraTransform.forward, out hit, 4 ,notPlayerCapsuleCollider))
                    {
                        //Подбор оружия
                        var usable = hit.collider.GetComponent<IUsable>();
                        if (usable != null)
                        {
                            switch (usable.GetUsableType())
                            {
                                case UsableType.Car:
                                    SitInCar(usable as CarController);
                                    return;
                                    break;
                                case UsableType.WeaponOnGround:
                                    WeaponOnGround weaponOnGround = usable as WeaponOnGround;
                                    if (weaponOnGround)
                                    {
                                        inventory.ChangeWeapon(weaponOnGround); 
                                    }
                                    break;
                                case UsableType.AmmoBox:
                                    //TODO
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.down * 0.2f, Color.blue);//Снова рисуем луч, только другого цвета, он никуда не попал
            }
            
            characterAnimations.SetIsGrounded(isGrounded);
        }

        private void CarMovement()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetOutFromCar();
                return;
            }

            car.Steering(Input.GetAxis("Horizontal"));
            float vertical = Input.GetAxis("Vertical");
            if (vertical >= 0)
            {
                car.AccelerationPedal(vertical);
                car.BrakePedal(0);
            }
            else
            {
                car.AccelerationPedal(0);
                car.BrakePedal(-vertical);
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                car.GearUp();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                car.GearDown();
            }
        }

        private void LateUpdate()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            cameraController.UpdateCamera(mouseX, mouseY);
        }

        private void ChangeAiming(bool isAiming)
        {
            characterAnimations.SetAiming(isAiming);
            cameraController.isAiming = isAiming;
            _isAiming = isAiming;
        }

        public void Jump()
        {
            rigidbody.AddForce(Vector3.up * jumpForce,ForceMode.VelocityChange);
        }
    }
}
