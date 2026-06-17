using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{

    public class Player : MonoBehaviour
    {
        #region Variables
        [Header("Movement Settings")]
        [SerializeField]
        private float moveSpeed = 5f; // 플레이어 이동 속도

        private Rigidbody2D rb2d;     // Rigidbody 2D 컴포넌트 참조 변수
        private float horizontalInput; // 입력받은 좌우 방향 값 (-1.0 ~ 1.0)
        #endregion

        #region Unity Lifecycle Events
        private void Awake()
        {
            // 오브젝트 내부의 Rigidbody 2D 컴포넌트를 가져와 참조를 연결합니다.
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            // 물리 연산 주기에 맞춰 플레이어를 이동시킵니다.
            Move();
        }
        #endregion

        #region Input System Events (Invoke Unity Events)
        /// <summary>
        /// Player Input 컴포넌트의 Invoke Unity Events에 의해 호출되는 메서드입니다.
        /// </summary>
        /// <param name="context">입력 시스템의 상태 정보</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            // InputSystem_Actions의 Move 액션은 Vector2 형태의 값을 반환합니다.
            Vector2 inputVector = context.ReadValue<Vector2>();

            // 그 중 좌우 이동에 해당하는 X축 값만 추출하여 저장합니다.
            horizontalInput = inputVector.x;
        }
        #endregion

        #region Movement Logic
        /// <summary>
        /// Rigidbody 2D의 velocity(속도)를 제어하여 물리적으로 플레이어를 이동시킵니다.
        /// </summary>
        private void Move()
        {
            // Y축 속도는 물리 엔진의 값(중력 등)을 그대로 유지하고, X축 속도만 입력값에 맞춰 변경합니다.
            rb2d.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb2d.linearVelocity.y);
        }
        #endregion
    }
}