using UnityEngine;

// 데미지를 입을 수 있는 타입들이 공통적으로 가져야 하는 인터페이스
namespace Interface
{
    public interface IDamageable 
    {
        // 데미지 크기, 맞은 지점, 맞은 표면의 방향 받는다
        void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
    }
}