using Animation;
using Item.Weapon;
using Managers;
using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
namespace Controller.PlayerController
{
    public class PlayerShooter : MonoBehaviour
    {
        public Gun gun; // 사용할 총
        public Transform gunPivot; // 총 배치의 기준점
        public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
        public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점
        
        private Animator _playerAnimator;

        private void Start()
        { 
            _playerAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        { 
            gun.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            gun.gameObject.SetActive(false);
        }

        public void Fire()
        {
            if (!gun.enabled) return;
            
            gun.Fire();
            UpdateUI();
        }

        public void Reload()
        {
            gun.Reload();
            _playerAnimator.SetTrigger(AnimationHashToParam.Reload);
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (gun != null && UIManager.Instance != null)
            { 
                // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
                UIManager.Instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
            }
        }

        // 애니메이터의 IK 갱신
        private void OnAnimatorIK(int layerIndex)
        {
            gunPivot.position = _playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);
            
            _playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            _playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            _playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
            _playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);
            
            _playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            _playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            _playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
            _playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
        }
    }
}