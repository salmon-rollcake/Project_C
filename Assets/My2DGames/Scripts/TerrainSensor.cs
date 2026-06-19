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

        // 접촉면 조건 설정
        [SerializeField] ContactFilter2D contFil;

        // 레이캐스트
        RaycastHit2D[] groundHits = new RaycastHit2D[5];

        // 바닥 체크
        bool isGround;
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
            IsGround = touchingColl.Cast(Vector2.down, contFil, groundHits, groundDis) > 0;
        }
        #endregion
    }
}