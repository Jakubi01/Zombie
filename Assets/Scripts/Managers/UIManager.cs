using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        // 싱글톤 접근용 프로퍼티
        public static UIManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindFirstObjectByType<UIManager>();
                }

                return _instance;
            }
        }

        private static UIManager _instance; // 싱글톤이 할당될 변수

        public Text ammoText; // 탄약 표시용 텍스트
        public Text scoreText; // 점수 표시용 텍스트
        public Text waveText; // 적 웨이브 표시용 텍스트
        public GameObject gameOverUI; // 게임 오버시 활성화할 UI

        // 탄약 텍스트 갱신
        public void UpdateAmmoText(int magAmmo, int remainAmmo)
        {
            ammoText.text = magAmmo + "/" + remainAmmo;
        }

        // 점수 텍스트 갱신
        public void UpdateScoreText(int newScore)
        {
            scoreText.text = "Score : " + newScore;
        }

        // 적 웨이브 텍스트 갱신
        public void UpdateWaveText(int waves, int count)
        {
            waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
        }

        // 게임 오버 UI 활성화
        public void SetActiveGameOverUI(bool active)
        {
            gameOverUI.SetActive(active);
        }

        // 게임 재시작 
        public void GameRestart()
        {
            GameManager.Instance?.RestartGame();
        }

        public void GameQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
