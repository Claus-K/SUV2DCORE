using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Cells
{
    public class MacrophageCellLogic : MonoBehaviour
    {
        // [SerializeField] private FieldOfView.Scripts.FieldOfView fov;
        private WhiteCell _white;

        private MeleeUnitComponent _muc; // melee unit component
        private NavMeshAgent _agent;
        private SelectionComponent _selectionComponent;
        private CombatComponentV2 _cbv2;

        [SerializeField] private float AttackRate;
        [SerializeField] private float minimunDistance;
        private bool hasHit;

        private Animator _animation;
        private static readonly int IsEngulfing = Animator.StringToHash("IsEngulfing");

        private float engulfCooldown = 10f;
        private float dt;

        private void Awake()
        {
            _white = new WhiteCell(10, 10, 2, 200)
            {
                _infection = GetComponent<InfectionComponent>(),
                _rbody2D = GetComponent<Rigidbody2D>()
            };
        }

        // Start is called before the first frame update
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _muc = GetComponent<MeleeUnitComponent>();
            _selectionComponent = GetComponent<SelectionComponent>();
            _cbv2 = GetComponent<CombatComponentV2>();
            ScenePoints.Instance.AddUnit(transform.gameObject, transform.GetComponent<CellTypeIdentifier>().Type);
            _animation = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_selectionComponent.AssignedMove)
            {
                // Move
                _white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                hasHit = false;
                _white.pathAgent.AssignedMove(_selectionComponent.destination, _agent);
                if (_white.pathAgent.HasReachedDestination(_agent))
                    _selectionComponent.AssignedMove = false;
            }
            else if (_selectionComponent.AssignedMoveAttack)
            {
                // Move Attack
                SeekTargetWrapper();
                if (!_white.TargetServiceScript.IsTargetValid(_white.target))
                {
                    _white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                    hasHit = false;
                    _white.pathAgent.AssignedMove(_selectionComponent.destination, _agent);
                    if (_white.pathAgent.HasReachedDestination(_agent))
                        _selectionComponent.AssignedMoveAttack = false;
                }
            }
            else if (_selectionComponent.AssignedAttack)
            {
                if (!_selectionComponent.assignedTarget)
                {
                    _white.target = null;
                    _selectionComponent.AssignedAttack = false;
                    _white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                    hasHit = false;
                }
                else
                {
                    // Assigned Attack
                    _white.target = _selectionComponent.assignedTarget;
                    SeekTargetWrapper(true);
                    if (!_white.TargetServiceScript.IsTargetInReach(transform, _selectionComponent.assignedTarget,
                            minimunDistance))
                    {
                        _white.pathAgent.AssignedAttack(_selectionComponent.assignedTarget.position, _agent, minimunDistance);
                        _white._rbody2D.bodyType = RigidbodyType2D.Dynamic;
                        hasHit = false;
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
            _muc.SeekTarget(ref _white._rbody2D, _white.TargetServiceScript, _agent, _white.pathAgent, _white.sightRange,
                AttackRate, ref _white.target, ref _white.searchTag, ref _white.overlapResults, ref hasHit,
                minimunDistance, assignedAttack, AttackTarget);
        }

        private void AttackTarget(Transform target)
        {
            if (Time.time - dt > engulfCooldown)
            {
                _animation.SetBool(IsEngulfing, true);
                dt = Time.time;
            }
            else
            {
                target.GetComponent<CombatComponentV2>().TakeDamage(_cbv2.Damage);
            }
        }

        private void OnDestroy()
        {
            ScenePoints.Instance.RemoveUnit(transform.gameObject.GetInstanceID(),
                transform.GetComponent<CellTypeIdentifier>().Type);
        }

        // animation event
        public void OnEngulfAnimationComplete()
        {
            _animation.SetBool(IsEngulfing, false);
        }
    }
}