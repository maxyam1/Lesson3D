using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vehicles;

namespace Ui
{
    public class CarUI : MonoBehaviour
    {
        [SerializeField] private float minRpmAngle = 200;
        [SerializeField] private float maxRpmAngle = 80;
        
        [SerializeField] private RectTransform ui;
        [SerializeField] private RectTransform linePrefab;
        [SerializeField] private RectTransform rpmNeedle;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI gearText;

        private List<RectTransform> spawnedLines = new List<RectTransform>();
        
        private static CarUI _instance;
        private static CarController _car;

        public static CarController Car
        {
            set
            {
                _car = value;
                if (value == null)
                {
                    _instance.ui.gameObject.SetActive(false);
                }
                else
                {
                    _instance.ChangeUI();
                }
            }
        }

        private void ChangeUI()
        {
            for (int i = spawnedLines.Count - 1; i >= 0; i--)
            {
                Destroy(spawnedLines[i].gameObject);
                spawnedLines.RemoveAt(i);
            }
            
            int linesNeeded = (int)Mathf.Ceil(_car.MaxEngineRpm / 1000f) + 1;

            float step = (maxRpmAngle - minRpmAngle) / linesNeeded; 
            
            for (int i = 0; i < linesNeeded; i++)
            {
                spawnedLines.Add(Instantiate(linePrefab, ui));
                //spawnedLines[i].anchoredPosition = Vector2.zero;
                float angle = minRpmAngle + (i + 1) * step;
                spawnedLines[i].rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void OnEnable()
        {
            _instance = this;
        }

        private void OnDisable()
        { 
            _instance = null;
        }

        void Update()
        {
            if (!_car)
            {
                return;
            }

            gearText.text = GearNumberToText(_car.Gear);
            speedText.text = $"{(int)_car.WheelSpeedKmh} kmh";
            float rpmNeedleAngle = minRpmAngle + (maxRpmAngle - minRpmAngle) * (_car.EngineRpm / _car.MaxEngineRpm);
            rpmNeedle.rotation = Quaternion.Euler(0, 0, rpmNeedleAngle);
        }

        private string GearNumberToText(int gear)
        {
            if (gear > 0)
            {
                return gear.ToString();
            }

            if (gear == 0)
            {
                return "N";
            }

            if (gear == -1)
            {
                return "R";
            }

            return "NAN";
        }
    }
}
