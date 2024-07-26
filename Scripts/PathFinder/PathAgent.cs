using UnityEngine;
using UnityEngine.AI;


namespace PathFinder
{
    public class PathAgent
    {
        private Vector3 lockedAgentPosition { get; set; }
        private bool isLockedAgent { get; set; }

        public Vector3 LockedAgent
        {
            get => lockedAgentPosition;
            set => lockedAgentPosition = value;
        }

        public bool IsLockedAget
        {
            get => isLockedAgent;
            set => isLockedAgent = value;
        }

        public void AssignedMove(Vector3 destination, NavMeshAgent agent)
        {
            agent.SetDestination(destination);
            SetRotation(agent.transform, destination);
            agent.stoppingDistance = 0.5f;
        }

        public void AssignedAttack(Vector3 destination, NavMeshAgent agent, float distance)
        {
            agent.SetDestination(destination);
            SetRotation(agent.transform, destination);
            agent.stoppingDistance = distance;
        }

        public void MoveTowards(Transform targetTransform, NavMeshAgent agent, bool canMove, float distance = 0.5f)
        {
            SetDestination(targetTransform, agent, canMove);
            agent.stoppingDistance = distance;
        }

        public bool HasReachedDestination(NavMeshAgent agent)
        {
            if (!(agent.remainingDistance <= agent.stoppingDistance) || agent.pathPending ||
                (!agent.hasPath && agent.velocity.sqrMagnitude != 0f)) return false;
            agent.ResetPath();
            return true;
        }

        private void SetDestination(Transform targetTransform, NavMeshAgent agent, bool canMove)
        {
            if (!targetTransform) return;
            var targetPosition = targetTransform.position;
            if (!canMove)
            {
                agent.SetDestination(targetPosition);
            }

            SetRotation(agent.transform, targetPosition);
        }

        private void SetRotation(Transform agentTransform, Vector3 targetPosition)
        {
            var direction = (targetPosition - agentTransform.position).normalized;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var currentAngle = agentTransform.rotation.eulerAngles.z;
            var newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * 5);

            agentTransform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        public bool IsFacingTarget(Transform agentTransform, Transform targetTransform, float tolerance = 2)
        {
            Vector3 direction =
                (targetTransform.position - agentTransform.position).normalized;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var currentAngle = agentTransform.rotation.eulerAngles.z;
            var angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

            return angleDifference <= tolerance;
        }
    }
}