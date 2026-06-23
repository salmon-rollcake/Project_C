using Unity.VisualScripting;
using UnityEngine;

namespace My2DGame
{

    public class Enemy : MonoBehaviour
    {
        Rigidbody2D rb;
        TerrainSensor sensor;
        Animator anim;

        // 플레이어 감지용 공격 판정 히트박스
        public HitBox hitbox;

        [SerializeField] float runSpeed = 5f;

        public bool isMove = true;

        // 이동 방향 제어 (1: 오른쪽, -1: 왼쪽)
        private int direction = 1;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sensor = GetComponent<TerrainSensor>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            // 1. 애니메이터의 isMove 파라미터 업데이트 (매 프레임 체크)
            if (anim != null)
            {
                bool shouldAnimateMove = isMove && !CannotMove;
                anim.SetBool(AnimationString.isMove, shouldAnimateMove);
            }

            // 2. 플레이어 감지 및 공격 로직 추가
            HandleAttack();
        }

        void FixedUpdate()
        {
            // 3. 벽을 감지하면 방향 전환
            if (sensor != null && sensor.IsGround && sensor.IsWall)
            {
                Flip();
            }

            // 4. 물리 기반 좌우 이동 처리
            if (isMove && !CannotMove)
            {
                rb.linearVelocity = new Vector2(direction * runSpeed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }

        /// <summary>
        /// 플레이어 감지 상태를 체크하고 공격 트리거를 발동하는 메서드
        /// </summary>
        private void HandleAttack()
        {
            if (hitbox == null || anim == null) return;

            // 히트박스에 무언가(플레이어) 감지되어 있다면 true
            bool hasTarget = hitbox.isDetected;

            // 애니메이터의 HasTarget 파라미터 갱신
            anim.SetBool(AnimationString.HasTarget, hasTarget);

            // 조건: 타겟이 있고, 현재 공격 쿨타임이 0.001보다 작다면 공격 수행
            if (hasTarget && CooldownTime < 0.001f)
            {
                // 애니메이터의 공격 트리거 발동
                anim.SetTrigger(AnimationString.AtkTrg);

                // ※ 참고: 보통 공격 애니메이션이 시작될 때 애니메이터 컨디션이나 
                // 스테이트 내부 스크립트(State Machine Behaviour), 혹은 별도의 타이머를 통해 
                // Cooldown 값을 다시 늘려주어야(예: 1.5f) 무한 공격을 막을 수 있습니다.
            }
        }

        #region Properties
        public bool CannotMove
        {
            get
            {
                if (anim == null) return false;
                return anim.GetBool(AnimationString.CannotMove);
            }
        }

        public float CooldownTime
        {
            get
            {
                if (anim == null) return 0f;
                return anim.GetFloat(AnimationString.Cooldown);
            }
            set
            {
                if (anim != null) anim.SetFloat(AnimationString.Cooldown, value);
            }
        }
        #endregion

        private void Flip()
        {
            direction *= -1;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}