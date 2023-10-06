using Cells;
using UnityEngine;

namespace PathFinder
{
    public class MovementUtility
    {
        private Vector2 randomDirection;
        private float timeToChangeDirection = 5f; // Time interval to change direction in seconds
        private float elapsedTime;

        public void MoveTowards(Transform currentTransform, Transform targetTransform, Rigidbody2D rb,
            float moveSpeed, bool stopMoving)
        {
            if (!targetTransform) return;
            if (stopMoving) return;
            Vector2 direction = (targetTransform.position - currentTransform.position).normalized;
            Vector2 moveAmount = direction * (moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + moveAmount);
        }

        public void MoveTowardsVirus(Transform currentTransform, Transform targetTransform, Rigidbody2D rb,
            float moveSpeed, bool stopMovement)
        {
            if (!targetTransform) return;
            if (stopMovement) return;
            var infection = targetTransform.GetComponent<InfectionComponent>();
            if (infection.infected) return;
            Vector2 direction = (targetTransform.position - currentTransform.position).normalized;
            Vector2 moveAmount = direction * (moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + moveAmount);
        }
        // Start is called before the first frame update

        public void WanderRandomly(Transform currentTransform, Rigidbody2D rb, float moveSpeed)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > timeToChangeDirection)
            {
                SetRandomDirection();
                elapsedTime = 0f;
            }

            if (elapsedTime < Random.Range(2f, 3f))
            {
                Vector2 moveAmount = randomDirection * (moveSpeed * Time.deltaTime);
                rb.MovePosition(rb.position + moveAmount);
            }
            
        }

        private void SetRandomDirection()
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad; // Convert degree to radians
            randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        }
    }
}