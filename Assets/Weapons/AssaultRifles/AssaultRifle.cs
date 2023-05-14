using System;
using System.Collections;
using UnityEngine;

namespace Weapons.AssaultRifles
{
    public class AssaultRifle : Weapon
    {
        [SerializeField] protected Renderer barrel;
        
        [SerializeField] protected float shootsPerMin;
        private float _shotCooldown;
        private bool _isReadyForShot = true;
        private bool _isTriggerPressed;

        [SerializeField] protected float maxTemperature = 950;
        [SerializeField] protected float temperaturePerShot = 30;
        [SerializeField] protected float coolingCoefficient = 20;
        public float _temperature = 0;
        private MaterialPropertyBlock _propertyBlock;
        private Color _barrelEmission = new Color(0,0,0);


        protected override void Start()
        {
            base.Start();
            _shotCooldown = 60 / shootsPerMin;
            
            _propertyBlock = new MaterialPropertyBlock ();
        }

        protected void Update()
        {
            Debug.DrawLine(shotPoint.position, shotPoint.position + shotPoint.forward * 100, Color.blue);

            CalculateTemperature();
            
            if (_isTriggerPressed && _isReadyForShot)
            {
                Shoot();
            }
        }

        private void CalculateTemperature()
        {
            _temperature -= (1 - (maxTemperature - _temperature)/maxTemperature) * coolingCoefficient * Time.deltaTime;

            float emission = Mathf.InverseLerp(750, maxTemperature, _temperature);

            _barrelEmission.r = emission;
            
            _propertyBlock.SetColor ("_EmissionColor", _barrelEmission);
            barrel.SetPropertyBlock (_propertyBlock);
        }

        protected override bool Shoot()
        {
            if (!base.Shoot())
                return false;
            _isReadyForShot = false;
            _temperature += temperaturePerShot;
            StartCoroutine(ShotCooldown());

            return true;
        }

        IEnumerator ShotCooldown()
        {
            float timeLeft = _shotCooldown;

            while (timeLeft > 0)
            {
                yield return new WaitForEndOfFrame();
                timeLeft -= Time.deltaTime;
            }

            _isReadyForShot = true;
        }

        public override void TriggerPressed()
        {
            _isTriggerPressed = true;
        }

        public override void TriggerUnPressed()
        {
            _isTriggerPressed = false;
        }
    }
}
