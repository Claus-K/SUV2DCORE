using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Cells
{
    public class BloodCellLogic : MonoBehaviour
    {
        public float areaRadius = 5f; // Radius of the area to wander within
        public float waitTime = 2f; // Time to wait before moving to the next point

        private NavMeshAgent agent;
        private SpriteRenderer spriteRenderer;
        private Vector3 initialPosition;
        private Vector3 lastPosition;

        private bool isAlive;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            initialPosition = transform.position;
            lastPosition = transform.position;
            ScenePoints.Instance.BloodIncDec(true);
            isAlive = true;
            StartCoroutine(WanderRoutine());
        }

        private void FixedUpdate()
        {
            FlipSpriteBasedOnDirection();
        }

        private void OnDestroy()
        {
            isAlive = false;
            ScenePoints.Instance.BloodIncDec(false);
        }

        private IEnumerator WanderRoutine()
        {
            
            while (isAlive)
            {
                Vector3 targetPosition = GetRandomPositionWithinRadius();
                agent.SetDestination(targetPosition);
                // Wait until the agent reaches the target position
                while (agent.pathPending || agent.remainingDistance > 0.5f)
                {
                    yield return null;
                }
                agent.ResetPath();
                // Wait for a specified time before moving to the next position
                yield return new WaitForSeconds(Random.Range(waitTime, 15f));
            }
        }

        private Vector3 GetRandomPositionWithinRadius()
        {
            Vector3 randomDirection = Random.insideUnitSphere * areaRadius;
            randomDirection.y = 0; // Ensure the y-coordinate remains the same
            randomDirection += initialPosition;

            NavMesh.SamplePosition(randomDirection, out var hit, areaRadius, 1);
        
            return hit.position;
        }

        private void FlipSpriteBasedOnDirection()
        {
            Vector3 direction = transform.position - lastPosition;
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Facing right
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Facing left
            }

            lastPosition = transform.position;
        }
    }
}