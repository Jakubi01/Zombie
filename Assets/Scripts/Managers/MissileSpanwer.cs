using System;
using UnityEngine;

namespace Managers
{
    public class MissileSpawner : MonoBehaviour
    {
        [Header("미사일 설정")]
        public GameObject missilePrefab;
        public float spawnHeight = 20f;

        [Header("범위 설정")]
        public float radius = 3f;

        public void SpawnMissile()
        {
            Vector3 origin = transform.position;
            Vector3 spawnPosition = new Vector3(
                origin.x,
                origin.y + spawnHeight,
                origin.z
            );

            Quaternion spawnRotation = Quaternion.LookRotation(Vector3.down); 
            Instantiate(missilePrefab, spawnPosition, spawnRotation);
        }
    }
}