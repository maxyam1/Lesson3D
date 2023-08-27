using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.SerializableDict;

namespace Vehicles
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private SuspensionType frontSuspensionType;
        [SerializeField] private SuspensionType rearSuspensionType;
        
        [SerializeField] private Transform wheelRenderer_FL;
        [SerializeField] private Transform wheelRenderer_FR;
        [SerializeField] private Transform wheelRenderer_RL;
        [SerializeField] private Transform wheelRenderer_RR;

        [SerializeField] private SerializableDictionary<CarDoor, CarDoorVIew> doorViewDictionary;

        public void SetWheelRenderersPosition()
        {
        }

        public CarDoorVIew GetDoor(CarDoor door)
        {
            if (!doorViewDictionary.ContainsKey(door))
            {
                return null;
            }

            return doorViewDictionary[door];
        }
    }
    
    public enum SuspensionType
    {
        Dependent,
        Independent
    }

    public enum CarDoor
    {
        Left,
        Right
    }
}
