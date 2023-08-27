using System;
using System.Collections;
using System.Collections.Generic;
using Ui;
using UnityEngine;
using UnityEngine.Serialization;

namespace Vehicles
{
    public class CarController : MonoBehaviour, IUsable
    {
        [SerializeField] private CarView carView;
        [SerializeField] private Transform seat;
        [SerializeField] private Transform centerOfGravity;
        [SerializeField] private float maxEngineTorque = 100;
        [SerializeField] private float maxEngineRpm = 6000;
        //[SerializeField] private float engineAndFlywheelMomentum = 1;
        //[SerializeField] private float engineBrakeTorque;
        [SerializeField] private AnimationCurve torqueCurve;
        //[SerializeField] private int gearCount = 5;
        [SerializeField] private List<float> frontGearRatios = new List<float>();
        [SerializeField] private float rearGearRatio = -30;
        [SerializeField] private float idleRpm;

        [SerializeField] private float maxBrakeTorque = 100;
        [SerializeField] private float frontRearBrakeRatio = 1.5f;

        [SerializeField] private WheelDriveType wheelDriveType;

        [SerializeField] private int avgRpmInterpolationSteps = 3;
        
        [SerializeField] protected WheelCollider wheelCollider_FL;
        [SerializeField] protected WheelCollider wheelCollider_FR;
        [SerializeField] protected WheelCollider wheelCollider_RL;
        [SerializeField] protected WheelCollider wheelCollider_RR;

        [SerializeField] private AudioSource engineAudioSource;
        [SerializeField] private AudioClip engineStartSound;
        [SerializeField] private AudioClip engineIdleSound;
        [SerializeField] private float engineIdleSoundRpm;
        [SerializeField] private AudioClip engineRpmDropSound;
        [SerializeField] private AudioClip engineStopSound;

        private Vector3 tmpPos;
        private Quaternion tmpRot;

        [Range(-1f, 1f)] private float _steeringWheelPos;
        [SerializeField] private float maxSteerAngle = 40;
        
        [Range(0f, 1f)] private float _accelerationPedalPos;
        private bool _accelerationPedalChanged;
        
        [Range(0f, 1f)] private float _brakePedalPos;
        private bool _brakePedalChanged;
        
        private bool _parkingBrake;

        private bool _engineOn;
        private bool _engineStarting;
        private float _currentEngineRpm = 1000;
        private int _currentGear = 0;
        private float _wheelSpeedKmh;

        private Queue<float> avgRpmLastStepsValues = new Queue<float>();

        public float EngineRpm => _currentEngineRpm;
        public float MaxEngineRpm => maxEngineRpm;
        public float WheelSpeedKmh => _wheelSpeedKmh;
        public int Gear => _currentGear;
        public Transform Seat => seat;

        public CarView CarView => carView;

        /// <summary>
        ///| 01 if fwd |
        ///| 10 if rwd |
        ///| 11 if awd |
        /// </summary>
        private enum WheelDriveType 
        {
            FWD = 1,
            RWD = 2,
            AWD = 3
        }


        private void Start()
        {
            //CarUI.Car = this;//Test
            
            if (_engineOn)
            {
                _currentEngineRpm = idleRpm;
            }
            else
            {
                _currentEngineRpm = 0;
            }

            GetComponent<Rigidbody>().centerOfMass = centerOfGravity.localPosition;

            //StartCoroutine(StartEngine());
        }

        public void TurnOnOffEngine(bool start)
        {
            if (start)
            {
                StartCoroutine(StartEngine());
            }
            else
            {
                StartCoroutine(StopEngine());
            }
        }

        private IEnumerator StartEngine()
        {
            if (_engineStarting)
            {
                yield break;
            }
            
            _engineOn = true;
            _engineStarting = true;
            engineAudioSource.clip = engineIdleSound;
            engineAudioSource.Stop();
            engineAudioSource.PlayOneShot(engineStartSound);
            engineAudioSource.loop = false;
            engineAudioSource.PlayScheduled(AudioSettings.dspTime + engineStartSound.length);
            
            yield return new WaitForSeconds(engineStartSound.length);

            _engineStarting = false;
            if (!_engineOn)
            {
                yield break;
            }

            _currentEngineRpm = idleRpm;
            engineAudioSource.loop = true;
        }

        private IEnumerator StopEngine()
        {
            engineAudioSource.clip = null;
            engineAudioSource.Stop();
            engineAudioSource.PlayOneShot(engineStopSound);
            _engineOn = false;
            _currentEngineRpm = 0;
            
            yield break;
        }
        
        void Update()
        {
            /*wheelCollider_FL.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_FL.position = tmpPos;
            wheelRenderer_FL.rotation = tmpRot;
           
            wheelCollider_FR.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_FR.position = tmpPos;
            wheelRenderer_FR.rotation = tmpRot;
            
            wheelCollider_RL.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_RL.position = tmpPos;
            wheelRenderer_RL.rotation = tmpRot;
            
            wheelCollider_RR.GetWorldPose(out tmpPos,out tmpRot);
            wheelRenderer_RR.position = tmpPos;
            wheelRenderer_RR.rotation = tmpRot;

            AudioCalculation();*/
        }

        private void FixedUpdate()
        {
            CarControl();
        }
        
        private void AudioCalculation()
        {
            if(_engineStarting || !_engineOn)
                return;

            engineAudioSource.pitch = _currentEngineRpm / engineIdleSoundRpm;
        }

        private void CarControl()
        {
            //Steering
            wheelCollider_FR.steerAngle = maxSteerAngle * _steeringWheelPos;
            wheelCollider_FL.steerAngle = maxSteerAngle * _steeringWheelPos;
            
            //Brake
            wheelCollider_FR.brakeTorque = _brakePedalPos * maxBrakeTorque;
            wheelCollider_FL.brakeTorque = _brakePedalPos * maxBrakeTorque;
            wheelCollider_RR.brakeTorque = _brakePedalPos * maxBrakeTorque / frontRearBrakeRatio;
            wheelCollider_RL.brakeTorque = _brakePedalPos * maxBrakeTorque / frontRearBrakeRatio;

            //ParkingBrake
            if (_parkingBrake)
            {
                wheelCollider_RR.brakeTorque += 1000;
                wheelCollider_RL.brakeTorque += 1000;   
            }

            WheelTorqueCalculation();
        }

        private void WheelTorqueCalculation()
        {
            if (_currentGear > frontGearRatios.Count || _currentGear < -1 || _currentGear == 0 || !_engineOn)// || _accelerationPedalPos == 0)
            {
                wheelCollider_FL.motorTorque = 0;
                wheelCollider_FR.motorTorque = 0;
                wheelCollider_RL.motorTorque = 0;
                wheelCollider_RR.motorTorque = 0;
                return;
            }

            int driveWheels = 2;

            if (wheelDriveType == WheelDriveType.AWD)
                driveWheels = 4;

            float currentAvgRpmOnWheels = 0;
            
            if (((int)wheelDriveType & 1) > 0)
            {
                currentAvgRpmOnWheels += wheelCollider_FL.rpm;
                currentAvgRpmOnWheels += wheelCollider_FR.rpm;
            }
            
            if (((int)wheelDriveType & 0b10) > 0)
            {
                currentAvgRpmOnWheels += wheelCollider_RL.rpm;
                currentAvgRpmOnWheels += wheelCollider_RR.rpm;
            }

            currentAvgRpmOnWheels /= driveWheels;
            
            
            avgRpmLastStepsValues.Enqueue(currentAvgRpmOnWheels);

            if (avgRpmLastStepsValues.Count > avgRpmInterpolationSteps)
            {
                Debug.LogError("Car interpolation queue overflow");
                avgRpmLastStepsValues.Clear();
                avgRpmLastStepsValues.Enqueue(currentAvgRpmOnWheels);
            }
            
            if (avgRpmLastStepsValues.Count == avgRpmInterpolationSteps)
            {
                avgRpmLastStepsValues.Dequeue();
            }
            
            float interpolatedAvgRpmOnWheels = 0;
            
            foreach (var value in avgRpmLastStepsValues)
            {
                interpolatedAvgRpmOnWheels += value;
            }
            
            interpolatedAvgRpmOnWheels /= avgRpmLastStepsValues.Count;

            float currentGearRatio = 0;
            if (_currentGear > 0)
            {
                currentGearRatio = frontGearRatios[_currentGear - 1];
            }
            else if(_currentGear < 0)
            {
                currentGearRatio = rearGearRatio;
            }

            _currentEngineRpm = Mathf.Clamp(interpolatedAvgRpmOnWheels * currentGearRatio, idleRpm, maxEngineRpm);

            if (_currentEngineRpm > maxEngineRpm)
            {
                wheelCollider_FL.motorTorque = 0;
                wheelCollider_FR.motorTorque = 0;
                wheelCollider_RL.motorTorque = 0;
                wheelCollider_RR.motorTorque = 0;
                return;
            }

            float engineTorque = torqueCurve.Evaluate(_currentEngineRpm / maxEngineRpm) * maxEngineTorque * _accelerationPedalPos;
            
            _wheelSpeedKmh = (interpolatedAvgRpmOnWheels / 60f) * wheelCollider_FL.radius * 2 * Mathf.PI * 3.6f;
            
            Debug.LogFormat("torque {0}, rpm {1}, km/h {2}, gear {3}", engineTorque, _currentEngineRpm, _wheelSpeedKmh, _currentGear);

            float totalWheelTorque = engineTorque * currentGearRatio;

            float torquePerWheel = totalWheelTorque / driveWheels;

            if (((int)wheelDriveType & 1) > 0)
            {
                wheelCollider_FL.motorTorque = torquePerWheel;
                wheelCollider_FR.motorTorque = torquePerWheel;
            }
            
            if (((int)wheelDriveType & 0b10) > 0)
            {
                wheelCollider_RL.motorTorque = torquePerWheel;
                wheelCollider_RR.motorTorque = torquePerWheel;
            }
        }

        public void Steering(float steeringWheelPos)
        {
            _steeringWheelPos = steeringWheelPos;
        }

        public void AccelerationPedal(float gasAxis)
        {
            _accelerationPedalPos = gasAxis;
            _accelerationPedalChanged = true;
        }

        public void BrakePedal(float brakeAxis)
        {
            _brakePedalPos = brakeAxis;
            _brakePedalChanged = true;
        }
        
        public void GearUp()
        {
            if (_currentGear < frontGearRatios.Count)
            {
                _currentGear++;
                if (_currentGear == 0)
                {
                    _currentEngineRpm = idleRpm;
                }
            }
        }
        
        public void GearDown()
        {
            if (_currentGear > -1)
            {
                _currentGear--;
                if (_currentGear == 0)
                {
                    _currentEngineRpm = idleRpm;
                }
            }
        }

        public void ParkingBrake(bool isBrake)
        {
            _parkingBrake = isBrake;
        }

        public void Use(CharacterController user)
        {
        }

        public UsableType GetUsableType()
        {
            return UsableType.Car;
        }
    }
}
