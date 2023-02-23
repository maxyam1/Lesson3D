using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float damage;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private ParticleSystem metalHitEffect;
        [SerializeField] private ParticleSystem sandHitEffect;
        [SerializeField] private ParticleSystem stoneHitEffect;
        [SerializeField] private ParticleSystem woodHitEffect;
        [SerializeField] private ParticleSystem[] meatHitEffect;

        private Vector3 _prevFramePos;

        private void Awake()
        {
            _prevFramePos = transform.position;
            Destroy(gameObject, 10f);
        }

        void Update()
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));

            RaycastHit hit;
            bool isCollision = Physics.Linecast(_prevFramePos, transform.position, out hit, layerMask);

            if (isCollision)
            {
                BulletHit(hit);
            }

            _prevFramePos = transform.position;
        }

        private void SpawnDecal(RaycastHit hit, ParticleSystem effectPrefab)
        {
            ParticleSystem spawnedEffect = Instantiate(effectPrefab, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));

            spawnedEffect.transform.parent = hit.transform;
            spawnedEffect.Play();
            
            Destroy(spawnedEffect, 10);//удаляем эффект
        }

        private void BulletHit(RaycastHit hit)
        {
            if (hit.collider.sharedMaterial != null)
            {
                switch (hit.collider.sharedMaterial.name)
                {
                    case "Metal":
                        SpawnDecal(hit, metalHitEffect);
                        break;
                    case "Wood":
                        SpawnDecal(hit, woodHitEffect);
                        break;
                    case "Sand":
                        SpawnDecal(hit, sandHitEffect);
                        break;
                    case "Stone":
                        SpawnDecal(hit, stoneHitEffect);
                        break;
                    case "Meat":
                        SpawnDecal(hit, meatHitEffect[Random.Range(0, meatHitEffect.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }

            Destroy(gameObject);//удаляем пулю
        }
    }
}
