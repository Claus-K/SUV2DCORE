using Enemy;
using PathFinder;
using Unity.Mathematics;
using UnityEngine;

namespace Cells
{
    public class BCell : MonoBehaviour
    {
        // Shooting related attributes
        public GameObject projectilePrefab;
        public float fireRate = 1f;
        public float launchForce = 3f;
        public string targetTag = "Virus";

        // Movement and detection related attributes
        public float detectionRange = 8.0f;
        public float moveSpeed = 2.0f;
        private Rigidbody2D rb;
        private float sightRange;
        private float minimumDistance = 7;
        private bool hitCollider;

        // Components and utilities
        public InfectionComponent infection;
        private Target objTarget;
        private Collider2D[] overlapResults = new Collider2D[10];
        private Transform target;
        private MovementUtility mov;
        

        private float dt;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            infection = GetComponent<InfectionComponent>();
            objTarget = new Target();
            target = objTarget.GetTarget(detectionRange, overlapResults, transform, targetTag);
            sightRange = detectionRange;
            mov = new MovementUtility();
        }

        private void FixedUpdate()
        {
            if (objTarget.isTargetValid(target, transform, sightRange))
            {
                MoveTowards(transform, target, rb, moveSpeed, hitCollider, minimumDistance);
                if (Time.time - dt >= fireRate)
                {
                    ShootAtTarget(target);
                    dt = Time.time;
                }
            }
            else
            {
                hitCollider = false;
                target = objTarget.GetTarget(detectionRange, overlapResults, transform, targetTag);
                Debug.Log(target);
            }

            if (infection.infected)
            {
                infection.EndInfection();
            }
        }

        public void MoveTowards(Transform currentTransform, Transform targetTransform, Rigidbody2D rb,
            float moveSpeed, bool stopMoving, float minimumDistance)
        {
            if (!targetTransform) return;
            if (stopMoving) return;

            float currentDistance = Vector2.Distance(currentTransform.position, targetTransform.position);

            if (currentDistance > minimumDistance)
            {
                Vector2 direction = (targetTransform.position - currentTransform.position).normalized;
                Vector2 moveAmount = direction * (moveSpeed * Time.deltaTime);
                rb.MovePosition(rb.position + moveAmount);
            }
        }

        private void ShootAtTarget(Transform target)
        {
            var position = transform.position;
            GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
            if (projectile == null)
            {
                Debug.Log("Failed to Create it");
            }
            Vector2 direction = (target.position - position).normalized;
            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            if (projectileRB != null)
            {
                projectileRB.velocity = direction * launchForce;
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (target == null) return;
            hitCollider = other.transform == target;
        }

        private void OnTriggerExit2D(Collider2D other) // Fixed the function signature
        {
            if (other.transform == target)
            {
                hitCollider = false;
            }
        }
    }
}