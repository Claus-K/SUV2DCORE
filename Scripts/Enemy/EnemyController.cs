using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyController2D : MonoBehaviour
    {
        public float detectionRange = 5.0f;
        public float moveSpeed = 2.0f;
        // rb2D + BoxCollider OR characterController which one to use? we go on with cc for now.
        private Rigidbody2D rb;
        private Transform target;
        private float reevaluationTime = 2f;
        private float dt;
        private Collider2D[] overlapResults = new Collider2D[3];

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            UpdateTarget();
        }

        private void FixedUpdate()
        {
            if (target)
            {
                MoveTowardsTarget();
                if (!(Time.time - dt > reevaluationTime)) return;
                UpdateTarget();
                dt = Time.time;
            }
            else
            {
                UpdateTarget();
            }
        }

        private void MoveTowardsTarget()
        {
            if (!target) return;
            Vector2 direction = (target.position - transform.position).normalized;
            Vector2 moveAmount = direction * (moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + moveAmount);
        }

        private void UpdateTarget()
        {
            float closestDistance = detectionRange;
            Transform closestTarget = null;
            // OverlapCircle to get all colliders within the detectionRange
            int iter = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRange, overlapResults);
            for (int i = 0; i < iter; i++)
            {
                // Check if the collider has the "Player" tag
                if (!overlapResults[i].gameObject.CompareTag("Player")) continue;
                float distanceToPlayer = Vector2.Distance(transform.position, overlapResults[i].transform.position);
                if (!(distanceToPlayer < closestDistance)) continue;
                closestDistance = distanceToPlayer;
                closestTarget = overlapResults[i].transform;
            }
            target = closestTarget;
        }
    }
}