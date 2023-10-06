using System;
using Cells;
using Unity.Mathematics;
using UnityEngine;

namespace PathFinder
{
    public class Target
    {
        public Transform GetTarget(float detectionRange, Collider2D[] overlapResults, Transform ownTransform,
            string tag)
        {
            float closestDistance = detectionRange;
            Transform closestTarget = null;
            // OverlapCircle to get all colliders within the detectionRange
            int iter = Physics2D.OverlapCircleNonAlloc(ownTransform.position, detectionRange, overlapResults);
            for (int i = 0; i < iter; i++)
            {
                if (!overlapResults[i].gameObject.CompareTag(tag)) continue;
                float distanceToPlayer = Vector2.Distance(ownTransform.position, overlapResults[i].transform.position);
                if (!(distanceToPlayer < closestDistance)) continue;
                closestDistance = distanceToPlayer;
                closestTarget = overlapResults[i].transform;
            }

            return closestTarget;
        }

        public Transform GetTargetVirus(float detectionRange, Collider2D[] overlapResults, Transform ownTransform,
            string tag)
        {
            float closestDistance = detectionRange;
            Transform closestTarget = null;
            // OverlapCircle to get all colliders within the detectionRange
            int iter = Physics2D.OverlapCircleNonAlloc(ownTransform.position, detectionRange, overlapResults);
            for (int i = 0; i < iter; i++)
            {
                var target = overlapResults[i].GetComponent<BaseCell>();
                if (target == null) continue;
                if (!overlapResults[i].gameObject.CompareTag(tag) && target.infection.infected) continue;
                float distanceToPlayer = Vector2.Distance(ownTransform.position, overlapResults[i].transform.position);
                if (!(distanceToPlayer < closestDistance)) continue;
                closestDistance = distanceToPlayer;
                closestTarget = overlapResults[i].transform;
            }

            return closestTarget;
        }

        // public bool Reevaluate(float currentTime, float dt, float reevTime)
        // {
        //     return currentTime - dt > reevTime;
        // }

        public bool isTargetValid(Transform target, Transform ownTransform, float distance)
        {
            if (target == null) return false;
            return Vector2.Distance(ownTransform.position, target.position) < distance;
            
        }
    }
}