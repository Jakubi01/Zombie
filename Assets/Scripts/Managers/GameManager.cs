using System;
using Characters.Enemy;
using Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Static Instance")]
        public static GameManager Instance { get; private set; }

        [Header("Game role")]
        private int _score;
        [SerializeField, Min(1)] private int killstreakActivationScore = 1000;

        public int Score => _score;
        public bool IsGameOver { get; set; }
        public bool IsKillstreakActive { get; private set; }
        public bool IsStandBy { get; set; }
        public event Action OnKillstreakActivated;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            SyncUI();

            var ms = FindFirstObjectByType<MissileSpawner>();
            if (ms)
            {
                OnKillstreakActivated += ms.SpawnMissile;
            }

            OnKillstreakActivated += UIManager.Instance.KillStreak;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                Instance = null;
            }
        }

        public void AddScore(int newScore)
        {
            if (IsGameOver) return;

            _score += newScore;
            UIManager.Instance?.UpdateScoreText(_score);

            if (!IsKillstreakActive && _score >= killstreakActivationScore)
            {
                IsKillstreakActive = true;
                OnKillstreakActivated?.Invoke();
            }
        }

        public void EndGame()
        {
            if (IsGameOver) return;

            IsGameOver = true;
            UIManager.Instance?.SetActiveGameOverUI(true);
        }

        public void RestartGame()
        {
            ResetSessionState();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void ResetSessionState()
        {
            _score = 0;
            IsGameOver = false;
            IsKillstreakActive = false;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SyncUI();
        }

        private void SyncUI()
        {
            if (!UIManager.Instance) return;

            UIManager.Instance.UpdateScoreText(_score);
            UIManager.Instance.SetActiveGameOverUI(IsGameOver);
        }

        public void KillAllEnemy(GameObject missile)
        {
            var sortedZombies = FindObjectsByType<Zombie>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var zombie in sortedZombies)
            {
                Debug.Log(zombie.name);
                var target = zombie.GetComponent<IDamageable>();
                target?.OnDamage(9999999999, Vector3.zero, Vector3.zero);
            }
            
            Destroy(missile);
        }
    }
}
