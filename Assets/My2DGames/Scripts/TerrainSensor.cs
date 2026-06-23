using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 접촉한 지형을 감지하는 클래스
    /// </summary>
    public class TerrainSensor : MonoBehaviour
    {
        #region Variables
        // 참조
        CapsuleCollider2D touchingColl;
        Animator anim;

        // 접촉면 체크
        [SerializeField] float groundDis = 0.05f;
        [SerializeField] float wallDis = 0.05f;

        // 접촉면 조건 설정
        [SerializeField] ContactFilter2D contFil;

        // 레이캐스트
        RaycastHit2D[] groundHits = new RaycastHit2D[5];
        RaycastHit2D[] wallHits = new RaycastHit2D[5];

        // 바닥 체크
        bool isGround;

        bool isWall;
        #endregion

        #region Property
        public bool IsGround
        {
            get
            {
                return isGround;
            }
            set
            {
                isGround = value;
                anim.SetBool(AnimationString.isGround, value);
            }
        }

        public bool IsWall
        {
            get
            {
                return isWall;
            }
            set
            {
                isWall = value;
                // 필요하다면 애니메이터에 벽 충돌 변수 전달
                // if (anim != null) anim.SetBool("isWall", value);
            }
        }
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            // 참조 연결
            touchingColl = GetComponent<CapsuleCollider2D>();
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            // 1. 바닥 감지 (아래 방향)
            IsGround = touchingColl.Cast(Vector2.down, contFil, groundHits, groundDis) > 0;

            // 2. 벽 감지 (바라보는 방향)
            // localScale.x가 양수이면 오른쪽(Vector2.right), 음수이면 왼쪽(Vector2.left)을 바라봅니다.
            Vector2 wallDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            // 바라보는 방향으로 캐스트를 쏘아 벽을 감지합니다.
            IsWall = touchingColl.Cast(wallDirection, contFil, wallHits, wallDis) > 0;
        }
        #endregion
    }
}