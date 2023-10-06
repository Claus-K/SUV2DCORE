using System;
using UnityEngine;

namespace Enemy
{
    public class EffectAntiBodyComponent : MonoBehaviour
    {
        public GameObject father;
        private EnemyController2D enemyController2D;
        private float dt;
        private float defaultMoveSpeed;

        void Start()
        {
            enemyController2D = father.GetComponent<EnemyController2D>();
            dt = Time.time;
            defaultMoveSpeed = enemyController2D.moveSpeed;
            enemyController2D.moveSpeed = (float)Math.Floor(enemyController2D.moveSpeed * 0.5f);
        }

        void FixedUpdate()
        {
            if (enemyController2D.infectFactor < 100)
            {
                if (Time.time - dt > 2f)
                {
                    enemyController2D.infectFactor = Mathf.Min(enemyController2D.infectFactor + 10, 100);
                    dt = Time.time;
                }
            }
            else
            {
                enemyController2D.moveSpeed = defaultMoveSpeed;
                transform.gameObject.SetActive(false);
            }
        }
    }
}