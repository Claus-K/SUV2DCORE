using System.Linq;
using UnityEngine;

namespace PathFinder
{
    public class Target
    {
        public Transform GetTarget(float detectionRange, Collider2D[] overlapResults, Transform ownTransform,
            string[] tags)
        {
            float closestDistance = detectionRange;
            Transform closestTarget = null;
            // OverlapCircle to get all colliders within the detectionRange
            int iter = Physics2D.OverlapCircleNonAlloc(ownTransform.position, detectionRange, overlapResults);
            for (int i = 0; i < iter; i++)
            {
                if (!tags.Contains(overlapResults[i].gameObject.tag)) continue;
                float distanceToPlayer = Vector2.Distance(ownTransform.position, overlapResults[i].transform.position);
                if (!(distanceToPlayer < closestDistance)) continue;
                closestDistance = distanceToPlayer;
                closestTarget = overlapResults[i].transform;
            }

            return closestTarget;
        }


        public bool isTargetValid(Transform target, Transform ownTransform, float distance)
        {
            if (target == null) return false;
            return Vector2.Distance(ownTransform.position, target.position) < distance;
        }
    }
}