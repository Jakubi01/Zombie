using Characters;
using UnityEngine;

namespace Controller
{
    public class Controller : MonoBehaviour
    {
        public GameObject owner;

        protected virtual void Awake()
        {
            owner = gameObject;
        }
    }
}