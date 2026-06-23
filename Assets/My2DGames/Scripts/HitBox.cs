using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace My2DGame
{

    public class HitBox : MonoBehaviour
    {
        List<Collider2D> Coll2D = new List<Collider2D>();

        public bool isDetected => Coll2D.Count > 0;

        public UnityAction noRemainColl; // 충돌체가 감지되었다가 없어졌을 때
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Coll2D.Add(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Coll2D.Remove(collision);

            if (Coll2D.Count == 0)
            {
                noRemainColl?.Invoke();
            }
        }
    }
}