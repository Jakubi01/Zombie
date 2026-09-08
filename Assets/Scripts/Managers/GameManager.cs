using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Static Instance")]
        public static GameManager Instance { get; private set; }

        [Header("Game role")]
        private int _score = 0;
        public bool IsGameOver { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        public void AddScore(int newScore)
        {
            if (IsGameOver) return;
            
            _score += newScore;
            UIManager.Instance.UpdateScoreText(_score);
        }
        
        public void EndGame()
        {
            IsGameOver = true;
            UIManager.Instance.SetActiveGameOverUI(true);
        }
    }
}