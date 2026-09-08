using Entity;
using Interface;
using UnityEngine;

// 체력을 회복하는 아이템
namespace Item.Interactable
{
    public class HealthPack : MonoBehaviour, IItem
    {
        public float health = 50; // 체력을 회복할 수치

        public void Use(GameObject target)
        {
            LivingEntity life = target.GetComponent<LivingEntity>();
            if (life != null)
            {
                life.RestoreHealth(health);
            }

            Destroy(gameObject);
        }
    }
}