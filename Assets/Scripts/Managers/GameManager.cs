using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Static Instance")]
        public static GameManager Instance => _instance;
        private static GameManager _instance;

        private void Awake()
        {
            if (!_instance)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }
}