using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 2D 배경 이미지에 카메라 이동에 따른 패럴렉스(원근감) 스크롤 효과를 부여하고,
    /// 이미지가 무한히 반복되도록 제어하는 컴포넌트입니다.
    /// </summary>
    public class ParallaxEffect : MonoBehaviour
    {
        #region 변수 선언
        [Header("카메라 설정")]
        [SerializeField]
        // [tooltip("추적할 메인 카메라의 트랜스폼입니다.")]
        private Transform cameraTransform;

        [Header("패럴렉스 옵션")]
        [SerializeField]
        // [tooltip("카메라 이동 대비 배경의 이동 속도 비율입니다. (0: 카메라와 함께 이동, 1: 완전히 고정)")]
        private float parallaxEffectMultiplier;

        // 무한 루프 계산을 위한 내부 변수
        private float startVelocityX;
        private float imageLengthX;
        #endregion

        /// <summary>
        /// 게임 시작 시 초기 위치와 이미지의 가로 크기를 계산합니다.
        /// </summary>
        private void Start()
        {
            // 카메라가 지정되지 않았다면 자동으로 메인 카메라를 찾아 할당합니다.
            if (cameraTransform == null)
            {
                if (Camera.main != null)
                {
                    cameraTransform = Camera.main.transform;
                }
            }

            // 스크립트가 부착된 오브젝트의 시작 X 좌표를 저장합니다.
            startVelocityX = transform.position.x;

            // SpriteRenderer를 통해 이미지의 순수 가로 크기(X축 길이)를 가져옵니다.
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                imageLengthX = spriteRenderer.bounds.size.x;
            }
        }

        /// <summary>
        /// 카메라의 이동이 완료된 후(LateUpdate) 배경의 위치를 재계산하여 떨림 현상을 방지합니다.
        /// </summary>
        private void LateUpdate()
        {
            if (cameraTransform == null) return;

            // 1. 패럴렉스 효과에 의해 배경이 실제로 이동해야 할 거리 계산
            float distanceToMove = cameraTransform.position.x * parallaxEffectMultiplier;

            // 2. 무한 루프(반복) 처리를 위해 카메라가 배경 이미지 기준 얼마나 이동했는지 계산
            float movementFactor = cameraTransform.position.x * (1 - parallaxEffectMultiplier);

            // 3. 배경 오브젝트의 위치를 업데이트 (Y축과 Z축은 고정, X축만 이동)
            transform.position = new Vector3(startVelocityX + distanceToMove, transform.position.y, transform.position.z);

            // 4. 무한 루프 스크롤 로직: 카메라가 이미지 중심축을 벗어나면 배경 위치를 한 칸 이동시킵니다.
            if (movementFactor > startVelocityX + imageLengthX)
            {
                startVelocityX += imageLengthX;
            }
            else if (movementFactor < startVelocityX - imageLengthX)
            {
                startVelocityX -= imageLengthX;
            }
        }
    }
}