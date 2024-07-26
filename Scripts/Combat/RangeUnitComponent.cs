using PathFinder;
using UnityEngine;
using UnityEngine.AI;

namespace Combat
{
    public class RangeUnitComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private GameObject _muzzle;
        private float launchForce = 3f;

        private float dt;

        public float LauchForce
        {
            // get => launchForce;
            set => launchForce = value;
        }

        public delegate void ActionMethod(GameObject muzzle, GameObject projectile, float force);

        // Takes in all relevant parameters, previous used inside each main script individualy.
        // NOTE :: target has to be 'ref', otherwise target from update reset it to null.
        public void SeekTarget(ref Rigidbody2D rb, TargetService targetServiceScript, NavMeshAgent agent, PathAgent pg,
            RangeUnitComponent ruc, float sightRange, float attackRate, ref Transform target, ref string[] searchTag,
            ref Collider2D[] overlapResults, ref bool hasHit, float minimunDistance, bool assignedAttack = false, ActionMethod action = null)
        {
            hasHit = pg.IsLockedAget;
            if (targetServiceScript.IsTargetValid(target, transform, sightRange))
            {
                pg.MoveTowards(target, agent, hasHit, minimunDistance);


                if (targetServiceScript.IsTargetInReach(transform, target, minimunDistance))
                {
                    if (rb.bodyType != RigidbodyType2D.Static)
                    {
                        pg.IsLockedAget = true;
                        hasHit = true;
                        rb.bodyType = RigidbodyType2D.Static;
                    }
                }
                else
                {
                    pg.IsLockedAget = false;
                    hasHit = false;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }

                if (Time.time - dt > attackRate && pg.IsFacingTarget(agent.transform, target))
                {
                    _muzzle.transform.localRotation = transform.rotation;
                    action?.Invoke(_muzzle, _projectilePrefab, launchForce);
                    dt = Time.time;
                }
            }
            else if (!assignedAttack)
            {
                hasHit = false;
                // target = targetScript.GetTarget(sightRange, overlapResults, transform,
                //     searchTag);
                if (!targetServiceScript.IsGettingTarget)
                {
                    StartCoroutine(targetServiceScript.FindTarget(sightRange, overlapResults, transform, searchTag,
                        targetServiceScript));
                    target = targetServiceScript.thyTarget;
                }
            }

            // Start a coroutine to check the target status if not already running
            if (!targetServiceScript.IsTargetChecking)
            {
                StartCoroutine(targetServiceScript.TargetCheck(target, rb, targetServiceScript, pg));
            }
        }
    }
}