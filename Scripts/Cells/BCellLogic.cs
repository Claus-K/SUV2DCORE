using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Cells
{
    public class BCellLogic : MonoBehaviour
    {
        private WhiteCell white; // all defensive cells

        private RangeUnitComponent _ruc; // range unit component
        private NavMeshAgent _agent;
        private SelectionComponent _selectionComponent;
        // private CombatComponentV2 _cbv2;

        private float attackRate = 1f;
        private float minimunDistance = 7f;
        private bool canMove;


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
            _agent = GetComponent<NavMeshAgent>();
            _ruc = GetComponent<RangeUnitComponent>();
            _ruc.LauchForce = 2.5f;
            _selectionComponent = GetComponent<SelectionComponent>();
            // _cbv2 = GetComponent<CombatComponentV2>();
            ScenePoints.Instance.AddUnit(transform.gameObject, transform.GetComponent<CellTypeIdentifier>().Type);
        }

        private void FixedUpdate()
        {
            if (_selectionComponent.AssignedMove)
            {
                // Move
                white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                canMove = false;
                white.pathAgent.AssignedMove(_selectionComponent.destination, _agent);
                if (white.pathAgent.HasReachedDestination(_agent)) _selectionComponent.AssignedMove = false;
            }
            else if (_selectionComponent.AssignedMoveAttack)
            {
                // Move Attack
                SeekTargetWrapper();
                if (!white.TargetServiceScript.IsTargetValid(white.target))
                {
                    white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                    canMove = false;
                    white.pathAgent.AssignedMove(_selectionComponent.destination, _agent);
                    if (white.pathAgent.HasReachedDestination(_agent)) _selectionComponent.AssignedMoveAttack = false;
                }
            }
            else if (_selectionComponent.AssignedAttack)
            {
                if (!_selectionComponent.assignedTarget)
                {
                    white.target = null;
                    _selectionComponent.AssignedAttack = false;
                    white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                    canMove = false;
                }
                else
                {
                    // Assigned Attack
                    white.target = _selectionComponent.assignedTarget;
                    SeekTargetWrapper(true);
                    if (!white.TargetServiceScript.IsTargetInReach(transform, _selectionComponent.assignedTarget, minimunDistance))
                    {
                        white.pathAgent.AssignedAttack(_selectionComponent.assignedTarget.position, _agent, minimunDistance);
                        white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                        canMove = false;
                    }
                }
            }
            else
            {
                SeekTargetWrapper();
            }
        }

        private void SeekTargetWrapper(bool assignedAttack = false)
        {
            _ruc.SeekTarget(ref white._rbody2D, white.TargetServiceScript, _agent, white.pathAgent, _ruc, white.sightRange, attackRate,
                ref white.target, ref white.searchTag, ref white.overlapResults, ref canMove, minimunDistance,
                assignedAttack, AttackTarget);
            // HOLY MOLLY
            // public void SeekTarget(Rigidbody2D rb, Target targetScript, NavMeshAgent agent, PathAgent pg,
            //     RangeUnitComponent ruc, float sightRange, float attackRate, ref Transform target, ref string[] searchTag,
            //     ref Collider2D[] overlapResults, ref bool hasHit, float minimunDistance, bool assignedAttack = false)
        }

        private void AttackTarget(GameObject muzzle, GameObject projectilePrefab, float force)
        {
            var position = muzzle.transform.position;
            var projectile = Instantiate(projectilePrefab, position, muzzle.transform.localRotation);
            Vector2 direction = projectile.transform.right.normalized;
            var projectileRB = projectile.GetComponent<Rigidbody2D>();
            projectileRB.velocity = direction * force;
        }

        private void OnDestroy()
        {
            // Debug.Log($" Tranform :: Destroy ID :: {transform.GetInstanceID()}");
            // Debug.Log($" GameObject :: Destroy ID :: {transform.gameObject.GetInstanceID()}");
            ScenePoints.Instance.RemoveUnit(transform.gameObject.GetInstanceID(),
                transform.GetComponent<CellTypeIdentifier>().Type);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (white.target != null)
            {
                canMove = other.transform == white.target;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            canMove = false;
        }
    }
}