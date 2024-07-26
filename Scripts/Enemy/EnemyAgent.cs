using Cells;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyAgent : MonoBehaviour
    {
        private Viruses _viruses;

        private MeleeUnitComponent _muc;
        private NavMeshAgent _agent;
        private SelectionComponent _selectionComponent;
        private CombatComponentV2 _cbv2;
        [SerializeField] private int infectFactor;

        public int InfectFactor
        {
            get => infectFactor;
            set => infectFactor = value;
        }

        [SerializeField] private int infectionCooldown = 6;

        private float attackRate = 1.6f;
        private float minimunDistance = 0.1f;
        private bool hasHit;

        [SerializeField] private int points;

        private float dt;


        private void Awake()
        {
            _viruses = new Viruses(10, 10, 2, 150)
            {
                _rb = GetComponent<Rigidbody2D>()
            };
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _viruses.MoveSpeed;
            _muc = GetComponent<MeleeUnitComponent>();
            _selectionComponent = GetComponent<SelectionComponent>();
            _cbv2 = GetComponent<CombatComponentV2>();
            // _selectionComponent.selectionCircle.GetComponent<SpriteRenderer>().color = Color.red;
            // _selectionComponent.selectionCircle.SetActive(false);
            if (_cbv2 == null)
            {
                Debug.LogError($"Combat Component is NULL");
            }
        }

        private void FixedUpdate()
        {
            if (_selectionComponent.AssignedMove)
            {
                _viruses._rb.bodyType = RigidbodyType2D.Dynamic;
                hasHit = false;
                _viruses.pg.AssignedMove(_selectionComponent.destination, _agent);
                if (_viruses.pg.HasReachedDestination(_agent))
                    _selectionComponent.AssignedMove = false;
            }
            else if (_selectionComponent.AssignedMoveAttack)
            {
                SeekTargetWrapper();
                if (!_viruses.TargetServiceScript.IsTargetValid(_viruses.target))
                {
                    _viruses._rb.bodyType = RigidbodyType2D.Dynamic;
                    hasHit = false;
                    _viruses.pg.AssignedMove(_selectionComponent.destination, _agent);
                    if (_viruses.pg.HasReachedDestination(_agent))
                        _selectionComponent.AssignedMoveAttack = false;
                }
            }
            else if (_selectionComponent.AssignedAttack)
            {
                if (!_selectionComponent.assignedTarget)
                {
                    _viruses.target = null;
                    _selectionComponent.AssignedAttack = false;
                    _viruses._rb.bodyType = RigidbodyType2D.Dynamic;
                    hasHit = false;
                }
                else
                {
                    _viruses.target = _selectionComponent.assignedTarget;
                    SeekTargetWrapper();
                    if (_viruses.TargetServiceScript.IsTargetInReach(transform, _selectionComponent.assignedTarget,
                            minimunDistance))
                    {
                        _viruses.pg.AssignedAttack(_selectionComponent.assignedTarget.position, _agent, minimunDistance);
                        _viruses._rb.bodyType = RigidbodyType2D.Dynamic;
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
            _muc.SeekTarget(ref _viruses._rb, _viruses.TargetServiceScript, _agent, _viruses.pg, _viruses.SightRange, attackRate,
                ref _viruses.target, ref _viruses.searchTag, ref _viruses.overlapResults, ref hasHit, minimunDistance,
                assignedAttack, AttackTarget);
        }


        private void AttackTarget(Transform target)
        {
            target.GetComponent<CombatComponentV2>().TakeDamage(_cbv2.Damage);
            if (InfectFactor == 0) return;
            if (Time.time - dt > infectionCooldown)
            {
                dt = Time.time;

                var infC = target.GetComponent<InfectionComponentV2>();
                if (infC.InfRes == 0)
                {
                    infC.StartInfection(GetComponent<EnemyVirusIdentifier>().Type);
                    Destroy(gameObject);
                    return;
                }

                var chance = Mathf.Max(InfectFactor - infC.InfRes, 0);
                var random = Random.Range(0, 100);
                if (random > chance) return;
                infC.StartInfection(GetComponent<EnemyVirusIdentifier>().Type);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            ScenePoints.Instance.Points += points;
            ScenePoints.Instance.VirusKillCount(transform.GetComponent<EnemyVirusIdentifier>().Type);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_viruses.target != null)
            {
                hasHit = other.transform == _viruses.target;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            hasHit = false;
        }
    }
}