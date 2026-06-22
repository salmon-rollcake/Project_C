using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{

    public class Player : MonoBehaviour
    {
        #region Variables
        [Header("Movement Settings")]
        [SerializeField]
        private float moveSpeed = 5f;        // 플레이어 기본 이동 속도
        [SerializeField]
        private float runSpeedMultiplier = 1.5f; // 달리기 속도 배율 (기본 속도의 1.5배)
        [SerializeField]
        private float jumpForce = 10f;       // 점프력

        private Rigidbody2D rb2d;            // Rigidbody 2D 컴포넌트 참조 변수
        private Animator anim;               // Animator 컴포넌트 참조 변수
        private float horizontalInput;       // 입력받은 좌우 방향 값 (-1.0 ~ 1.0)
        private bool isRunning;              // 현재 달리고 있는 상태인지 여부

        // 애니메이터 파라미터 최적화를 위한 해시 ID 변수
        private readonly int isMoveHash = Animator.StringToHash("isMove");
        private readonly int isRunHash = Animator.StringToHash("isRun");
        private readonly int attackHash = Animator.StringToHash("AtkTrg"); // 공격 트리거 파라미터
        private readonly int jumpHash = Animator.StringToHash("JumpTrg");

        private TerrainSensor terrainSensor;
        #endregion

        #region Unity Lifecycle Events
        private void Awake()
        {
            // 오브젝트 내부의 필수 컴포넌트들을 가져와 참조를 연결합니다.
            rb2d = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            terrainSensor = GetComponent<TerrainSensor>();
        }

        private void Update()
        {
            // 애니메이션 상태를 매 프레임 업데이트합니다.
            UpdateAnimation();

            // 입력 방향에 맞춰 플레이어의 시선(Scale X)을 방향 전환합니다.
            FlipSprite();
        }

        private void FixedUpdate()
        {
            // 물리 연산 주기에 맞춰 플레이어를 이동시킵니다.
            Move();
        }
        #endregion

        #region New Input System Events
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            horizontalInput = inputVector.x;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            isRunning = context.performed;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Jump();
                anim.SetTrigger(jumpHash);
            }
        }

        /// <summary>
        /// New Input System의 Attack 액션과 연결되어 공격 키 입력을 처리합니다.
        /// </summary>
        public void OnAttack(InputAction.CallbackContext context)
        {
            // 버튼을 누른 바로 그 순간(started)에만 실행합니다.
            if (context.started)
            {
                // 현재 공격 애니메이션이 재생 중이 아닐 때만 공격을 실행합니다.
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    anim.SetTrigger(attackHash);
                }
            }
        }
        #endregion

        #region Movement Logic
        private void Move()
        {
            // (선택 사항) 공격 중일 때 이동을 멈추게 하고 싶다면 아래 주석을 해제하세요.
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
                return;
            }

            float currentSpeed = isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;
            rb2d.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb2d.linearVelocity.y);
        }

        private void Jump()
        {
            float currentJumpForce = isRunning ? jumpForce * runSpeedMultiplier : jumpForce;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, currentJumpForce);
        }

        private void FlipSprite()
        {
            // 공격 중일 때는 시선 방향을 전환하지 않고 함수를 빠져나갑니다.
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                return;
            }

            if (horizontalInput > 0f)
            {
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            }
            else if (horizontalInput < 0f)
            {
                transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            }
        }
        #endregion

        #region Animation Logic
        private void UpdateAnimation()
        {
            // 💡 핵심: 현재 애니메이터가 "Attack" 상태를 재생 중이라면, 
            // 아래의 이동 애니메이션(isMove, isRun) 갱신 로직을 무시하고 함수를 빠져나갑니다.
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }
            /*
            bool isGrounded = terrainSensor.IsGround;
            anim.SetBool("isGround", isGrounded);
            */
            bool isMoving = horizontalInput != 0f;
            anim.SetBool(isMoveHash, isMoving);
            anim.SetBool(isRunHash, isMoving && isRunning);
            anim.SetFloat("yVelocity", rb2d.linearVelocity.y);
        }
        #endregion
    }
}