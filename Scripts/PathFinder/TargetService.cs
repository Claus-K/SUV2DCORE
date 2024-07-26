using System.Collections;
using System.Linq;
using UnityEngine;

namespace PathFinder
{
    public class TargetService
    {
        private bool isTargetChecking { get; set; }
        private bool isGettingTarget { get; set; }

        private Transform target { get; set; }

        public bool IsTargetChecking
        {
            get => isTargetChecking;
            set => isTargetChecking = value;
        }

        public bool IsGettingTarget
        {
            get => isGettingTarget;
            set => isGettingTarget = value;
        }

        public Transform thyTarget
        {
            get => target;
            set => target = value;
        }


        public Transform FindTarget(float detectionRange, Collider2D[] overlapResults, Transform ownTransform,
            string[] allowedTags)
        {
            Transform closestTarget = null;
            var closestDistance = detectionRange;
            // Perform an overlap circle using the specified layer mask
            var iter = Physics2D.OverlapCircleNonAlloc(ownTransform.position, detectionRange, overlapResults,
                LayerMask.GetMask("Agents"));

            for (int i = 0; i < iter; i++)
            {
                var collider = overlapResults[i];
                if (!allowedTags.Contains(collider.gameObject.tag)) continue;
                var distanceToTarget = Vector2.Distance(ownTransform.position, collider.transform.position);
                if (!(distanceToTarget < closestDistance)) continue;
                closestDistance = distanceToTarget;
                closestTarget = collider.transform;
            }

            return closestTarget;
        }

        public bool IsTargetValid(Transform targetTransform, Transform ownTransform, float distance)
        {
            if (!targetTransform) return false;
            return Vector2.Distance(ownTransform.position, targetTransform.position) < distance;
        }

        public bool IsTargetValid(Transform targetTransform)
        {
            return targetTransform;
        }

        public bool IsTargetInReach(Transform ownTransform, Transform targetTransform, float distance)
        {
            var difference = targetTransform.position - ownTransform.position;
            var squaredDistance = difference.sqrMagnitude;
            return squaredDistance <= distance * distance;
        }

        public IEnumerator FindTarget(float detectionRange, Collider2D[] overlapResults, Transform ownTransform,
            string[] allowedTags, TargetService targetServiceScript)
        {
            if (targetServiceScript.IsGettingTarget) yield break;

            targetServiceScript.IsGettingTarget = true;
            targetServiceScript.thyTarget = FindTarget(detectionRange, overlapResults, ownTransform, allowedTags);
            yield return new WaitForSeconds(0.5f);
            targetServiceScript.IsGettingTarget = false;
        }

        public IEnumerator TargetCheck(Transform targetTransform, Rigidbody2D rb, TargetService targetServiceScript, PathAgent pg)
        {
            targetServiceScript.IsTargetChecking = true;
            yield return new WaitForSeconds(1.0f);
            if (!targetTransform)
            {
                pg.IsLockedAget = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            targetServiceScript.IsTargetChecking = false;
        }
    }
}