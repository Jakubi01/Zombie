using UnityEngine;

namespace Interface
{
    public interface IItem 
    {
        // target은 아이템 효과가 적용될 대상
        void Use(GameObject target);
    }
}