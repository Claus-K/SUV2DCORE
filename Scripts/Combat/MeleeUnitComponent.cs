using PathFinder;
using UnityEngine;
using UnityEngine.AI;

namespace Combat
{
    public class MeleeUnitComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _muzzle;
        [SerializeField] private float Damage;
        private float dt;

        private void AttackTarget(Transform target)
        {
            target.GetComponent<CombatComponent>().TakeDamage(Damage);
        }

        public delegate void ActionMethod(Transform target);

        public void SeekTarget(ref Rigidbody2D rbody2D, TargetService targetServiceScript, NavMeshAgent agent, PathAgent pathAgent,
             float sightRange, float attackRate, ref Transform target, ref string[] searchTag,
            ref Collider2D[] overlapResults, ref bool canMove, float minimunDistance, bool assignedAttack = false,
            ActionMethod action = null)
        {
            canMove = pathAgent.IsLockedAget;
            if (targetServiceScript.IsTargetValid(target, transform, sightRange))
            {
                minimunDistance += target.GetComponent<CircleCollider2D>().radius * 2;
                pathAgent.MoveTowards(target, agent, canMove, minimunDistance);

                // Body to Static & Lock Movement.
                if (targetServiceScript.IsTargetInReach(transform, target, minimunDistance))
                {
                    
                    if (rbody2D.bodyType != RigidbodyType2D.Static)
                    {
                        pathAgent.IsLockedAget = true;
                        canMove = true;
                        rbody2D.bodyType = RigidbodyType2D.Static;
                    }
                }
                
                else
                {
                    pathAgent.IsLockedAget = false;
                    canMove = false;
                    rbody2D.bodyType = RigidbodyType2D.Dynamic;
                }

                if (Time.time - dt > attackRate && pathAgent.IsFacingTarget(agent.transform, target) && canMove)
                {
                    _muzzle.transform.localRotation = transform.rotation;
                    if (action != null)
                        action.Invoke(target);
                    else
                        AttackTarget(target);


                    dt = Time.time;
                }
            }
            else if (!assignedAttack)
            {
                canMove = false;
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
                StartCoroutine(targetServiceScript.TargetCheck(target, rbody2D, targetServiceScript, pathAgent));
            }
        }
    }
}