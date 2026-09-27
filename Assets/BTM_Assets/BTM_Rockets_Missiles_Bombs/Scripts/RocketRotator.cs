using Managers;
using UnityEngine;

namespace BTM_Assets.BTM_Rockets_Missiles_Bombs.Scripts
{
    public class RocketRotator : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private Vector3 rotationAngle = new Vector3(0, 45, 0);
        [SerializeField] private bool isRotating = true;
        
        [Header("Particle")]
        public GameObject explosionParticle;

        private void Awake()
        {
            transform.Rotate(90f, 0f, 0f);
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        
        private void Update()
        {
            if (isRotating)
            {
                RotateObject();
            }
        }

        private void RotateObject()
        {
            transform.Rotate(rotationAngle * Time.deltaTime, Space.Self);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name != "Ground") return;
            
            if(explosionParticle)
                Instantiate(explosionParticle, gameObject.transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            GameManager.Instance.KillAllEnemy(gameObject);
        }
    }
}