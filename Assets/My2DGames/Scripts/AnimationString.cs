using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 애니메이터 파라미터 이름 리스트 - 전역적 접근 가능
    /// </summary>
    public class AnimationString : MonoBehaviour
    {
        public static string isMove = "IsMove";
        public static string isRun = "IsRun";
        public static string isGround = "IsGround";
        public static string JumpTrg = "jumpTrg";
        public static string yVelocity = "YVelocity";
    }
}