using UnityEngine;

namespace Cells
{
    public class BCell : MonoBehaviour
    {
        public WhiteCell white;

        // Shooting related attributes
        public GameObject projectilePrefab;
        public float attackRate = 1f;
        public float launchForce = 3f;
        private float minimumDistance = 7;
        private float dt;

        private bool hasHit;

        private void Awake()
        {
            white = new WhiteCell(10, 10, 2, 100f)
            {
                _infection = GetComponent<InfectionComponent>(),
                _rbody2D = GetComponent<Rigidbody2D>()
            };
        }

        private void Start()
        {
            // if resist not informed is set to 30
            dt = Time.time;
        }

        private void FixedUpdate()
        {
            if (white.TargetServiceScript.IsTargetValid(white.target, transform, white.sightRange))
            {
                white.movmentUtilitys.RangeMoveTowards(transform, white.target, white._rbody2D, white.moveSpeed, hasHit, minimumDistance);

                if (Time.time - dt > attackRate)
                {
                    ShootAtTarget(white.target);
                    dt = Time.time;
                }
            }
            else
            {
                hasHit = false;
                white.target =
                    white.TargetServiceScript.FindTarget(white.detectionRange, white.overlapResults, transform, white.searchTag);
            }

            if (white._infection.infected)
            {
                white._infection.EndInfection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (white.target != null)
            {
                hasHit = other.transform == white.target;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            hasHit = false;
        }

        private void ShootAtTarget(Transform target)
        {
            var position = transform.position;
            var projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
            Vector2 direction = (target.position - position).normalized;
            var projectileRB = projectile.GetComponent<Rigidbody2D>();
            projectileRB.velocity = direction * launchForce;
        }
    }
}