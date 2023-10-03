using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyController2D : MonoBehaviour
    {
        public float detectionRange = 5.0f; 
        public float moveSpeed = 2.0f; 
        
        private Rigidbody2D rb; // Reference to the enemy's rigidbody2D

        private Transform target;
        private float reevaluationTime = 2f;
        private float dt;

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
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }

        private void UpdateTarget()
        {
            float closestDistance = detectionRange;
            Transform closestTarget = null;

            // Use OverlapCircleAll to get all colliders within the detectionRange
            var colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);

            foreach (Collider2D collider in colliders)
            {
                // Check if the collider has the "Player" tag
                if (collider.gameObject.CompareTag("Player"))
                {
                    float distanceToPlayer = Vector2.Distance(transform.position, collider.transform.position);
                    if (distanceToPlayer < closestDistance)
                    {
                        closestDistance = distanceToPlayer;
                        closestTarget = collider.transform;
                    }
                }
            }

            target = closestTarget;
        }
    }
}