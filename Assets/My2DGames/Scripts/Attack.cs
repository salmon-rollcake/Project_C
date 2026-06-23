using UnityEngine;

namespace My2DGame
{

    public class Attack : MonoBehaviour
    {
        [SerializeField] float atkDamage = 10f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DamageDeal dDeal = collision.GetComponent<DamageDeal>();

            if (dDeal != null)
            {
                dDeal.TakeDamage(atkDamage);

                Transform dirTrans = transform.parent != null ? transform.parent : transform;

            }
        }
    }
}