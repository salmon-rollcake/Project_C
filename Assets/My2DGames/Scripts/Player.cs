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

        private Rigidbody2D rb2d;            // Rigidbody 2D 컴포넌트 참조 변수
        private Animator anim;               // Animator 컴포넌트 참조 변수
        private float horizontalInput;       // 입력받은 좌우 방향 값 (-1.0 ~ 1.0)
        private float verticalInput;
        private bool isRunning;              // 현재 달리고 있는 상태인지 여부
        private bool isJumping;

        [SerializeField] float jumpForce = 10f;

        // 애니메이터 파라미터 최적화를 위한 해시 ID 변수
        private readonly int isMoveHash = Animator.StringToHash("isMove");
        private readonly int isRunHash = Animator.StringToHash("isRun");
        #endregion

        #region Unity Lifecycle Events
        private void Awake()
        {
            // 오브젝트 내부의 필수 컴포넌트들을 가져와 참조를 연결합니다.
            rb2d = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
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
            Jump();
        }
        #endregion

        #region New Input System Events
        /// <summary>
        /// New Input System의 Move 액션과 연결되어 좌우 입력값을 받아오는 메서드입니다.
        /// </summary>
        /// <param name="context">입력 시스템의 상태 정보</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            // Input Action의 Control Type이 Vector 2이므로 Vector2로 값을 읽어와 X축(좌우) 값만 저장합니다.
            Vector2 inputVector = context.ReadValue<Vector2>();
            horizontalInput = inputVector.x;
        }

        /// <summary>
        /// New Input System의 Sprint 액션과 연결되어 달리기 키 상태를 받아오는 메서드입니다.
        /// </summary>
        /// <param name="context">입력 시스템의 상태 정보</param>
        public void OnSprint(InputAction.CallbackContext context)
        {
            // 키를 누르고 있는 상태(Started, Performed)일 때는 true, 키를 떼었을 때(Canceled)는 false가 됩니다.
            isRunning = context.performed;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // Input Action의 Control Type이 Vector 2이므로 Vector2로 값을 읽어와 X축(좌우) 값만 저장합니다.
            Vector2 inputVector = context.ReadValue<Vector2>();
            verticalInput = inputVector.y;
        }
        #endregion

        #region Movement Logic
        /// <summary>
        /// Rigidbody 2D의 velocity(속도)를 제어하여 플레이어를 이동시키는 메서드입니다.
        /// </summary>
        private void Move()
        {
            // 현재 달리기 상태에 따라 최종 이동 속도를 계산합니다.
            float currentSpeed = isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;

            // Rigidbody 2D를 통해 플레이어의 물리 속도를 변경합니다.
            rb2d.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb2d.linearVelocity.y);
        }

        private void Jump()
        {
            // 현재 달리기 상태에 따라 최종 이동 속도를 계산합니다.
            float currentSpeed = isJumping ? jumpForce * runSpeedMultiplier : jumpForce;

            // Rigidbody 2D를 통해 플레이어의 물리 속도를 변경합니다.
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, verticalInput * currentSpeed);
        }

        /// <summary>
        /// 플레이어의 좌우 입력값(horizontalInput)을 바탕으로 캐릭터의 시선 방향(Scale X)을 전환하는 메서드입니다.
        /// </summary>
        private void FlipSprite()
        {
            // 오른쪽 입력(> 0)이 들어오면 원래 상태인 1로, 왼쪽 입력(< 0)이 들어오면 반전 상태인 -1로 설정합니다.
            // 입력이 없는 상태(0)일 때는 기존에 바라보던 방향을 그대로 유지하도록 조건문을 구성합니다.
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
        /// <summary>
        /// 플레이어의 이동 및 달리기 상태를 기반으로 애니메이터 파라미터를 실시간 제어하는 메서드입니다.
        /// </summary>
        private void UpdateAnimation()
        {
            // 좌우 입력값(horizontalInput)이 0이 아니라면 움직이고 있는 상태입니다.
            bool isMoving = horizontalInput != 0f;

            // 애니메이터의 파라미터 값들을 갱신합니다.
            anim.SetBool(isMoveHash, isMoving);
            anim.SetBool(isRunHash, isMoving && isRunning);
        }
        #endregion
    }
}