using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 캐릭터의 체력을 관리하는 클래스
    /// </summary>
    public class DamageDeal : MonoBehaviour
    {
        #region Variables
        // 참조
        Animator anim;

        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        //죽음 체크
        private bool isDeath = false;
        #endregion

        #region Property
        public float MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            private set { currentHealth = value; }
        }

        public bool IsDeath
        {
            get { return IsDeath; }
            private set
            {
                isDeath = value;
                anim.SetBool(AnimationString.isDeath, value);
            }
        }

        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            //초기화
            CurrentHealth = MaxHealth;
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage)
        {

        }
        #endregion
    }
}