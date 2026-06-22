using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 애니메이터 파라미터 이름 리스트 - 전역적 접근 가능
    /// </summary>
    public class AnimationString : MonoBehaviour
    {
        public static string isMove = "isMove";
        public static string isRun = "isRun";
        public static string isGround = "isGround";
        public static string JumpTrg = "JumpTrg";
        public static string yVelocity = "yVelocity";
        public static string AtkTrg = "AtkTrg";
    }
}