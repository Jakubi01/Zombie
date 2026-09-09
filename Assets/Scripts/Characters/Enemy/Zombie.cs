using System.Collections;
using Animation;
using Entity;
using UnityEngine;
using UnityEngine.AI;


namespace Characters.Enemy
{
    public class Zombie : LivingEntity
    {
        public LayerMask whatIsTarget; // 추적 대상 레이어

        private LivingEntity _targetEntity; // 추적 대상
        private NavMeshAgent _navMeshAgent; // 경로 계산 AI 에이전트

        public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
        public AudioClip deathSound; // 사망 시 재생할 소리
        public AudioClip hitSound; // 피격 시 재생할 소리

        private Animator _zombieAnimator; // 애니메이터 컴포넌트
        private AudioSource _zombieAudioPlayer; // 오디오 소스 컴포넌트
        private Renderer _zombieRenderer; // 렌더러 컴포넌트

        public float damage = 20f; // 공격력
        public float timeBetAttack = 0.5f; // 공격 간격
        private float _lastAttackTime; // 마지막 공격 시점
        private const float TurnSpeed = 10f;
        
        // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
        private bool HasTarget => _targetEntity && !_targetEntity.Dead;
        
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _zombieAnimator = GetComponent<Animator>();
            _zombieAudioPlayer = GetComponent<AudioSource>();
            _zombieRenderer = GetComponentInChildren<Renderer>();
        }

        // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
        public void Setup(ZombieData zombieData)
        {
            startingHealth = zombieData.health;
            Health = startingHealth;
            damage = zombieData.damage;
            _navMeshAgent.speed = zombieData.speed;
            _navMeshAgent.stoppingDistance = 1f;
            _zombieRenderer.material.color = zombieData.skinColor;
        }

        private void Start()
        {
            // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
            StartCoroutine(UpdatePath());
        }

        private void Update()
        {
            // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
            _zombieAnimator.SetBool(AnimationHashToParam.HasTarget, HasTarget);
            
            if (!HasTarget || !_targetEntity || Dead) return;
                
            Vector3 direction = _targetEntity.transform.position - transform.position;
            direction.y = 0f;

            if (direction == Vector3.zero) return;
                
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        }

        // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
        private IEnumerator UpdatePath()
        {
            // 살아 있는 동안 무한 루프
            while (!Dead)
            {
                if (HasTarget)
                {
                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.SetDestination(_targetEntity.transform.position);
                }
                else
                {
                    _navMeshAgent.isStopped = true;

                    var results = new Collider[1];
                    var size = Physics.OverlapSphereNonAlloc(transform.position, 20f, results, whatIsTarget);
                    if (size > 0)
                    {
                        foreach (var result in results)
                        {
                            var livingEntity = result.GetComponent<LivingEntity>();
                            if (!livingEntity || livingEntity.Dead) continue;
                            
                            _targetEntity = livingEntity;
                            break;
                        }
                    }
                }
                
                // 0.25초 주기로 처리 반복
                yield return new WaitForSeconds(0.25f);
            }
        }

        // 데미지를 입었을 때 실행할 처리
        public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (!Dead)
            {
                hitEffect.transform.position = hitPoint;
                hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
                hitEffect.Play();
                
                _zombieAudioPlayer.PlayOneShot(hitSound);
            }
            
            base.OnDamage(damage, hitPoint, hitNormal);
        }

        // 사망 처리
        public override void Die()
        {
            // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
            base.Die();
            
            var zombieColliders = GetComponents<Collider>();
            foreach (var zombieCollider in zombieColliders)
            {
                zombieCollider.enabled = false;
            }
            
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
            
            _zombieAnimator.SetTrigger(AnimationHashToParam.Die);
            _zombieAudioPlayer.PlayOneShot(deathSound);
        }

        private void OnTriggerStay(Collider other)
        {
            // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
            if (!Dead && Time.time >= _lastAttackTime + timeBetAttack)
            {
                var attackTarget = other.GetComponent<LivingEntity>();
                if (!attackTarget || attackTarget != _targetEntity) return;
                
                _lastAttackTime = Time.time;
                var hitPoint = other.ClosestPoint(transform.position);
                var hitNormal = hitPoint - other.transform.position;
                    
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 20f);
        }
    }
}
