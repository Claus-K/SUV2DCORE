using System.Linq;
using Combat;
using PathFinder;
using Resources;
using UnityEngine;

namespace Cells
{
    public class Dendritic : MonoBehaviour
    {
        private WhiteCell white;
        private float dt;
        private float attackRate = 1f;

        private Collider2D[] overlapResults = new Collider2D[3];
        private string[] searchTag;

        private Rigidbody2D _rb;
        private Target _target;
        private Transform target;

        private bool hasHit;

        private MovementUtility mov;

        private Transform turnCell;

        [SerializeField] private CellDatabase _cellDatabase;

        private enumCellType turnCellId;
        private bool turnTarget;
        private CombatComponent _cb;
        private int rnaPoints;
        public ResourceManager _resourceManager;


        private void Awake()
        {
            white = new WhiteCell(6, 6, 2, 150f)
            {
                _infection = GetComponent<InfectionComponent>()
            };
            // searchTag = white.searchTag.Concat(new[] { "VirusCorpse" }).ToArray();
            searchTag = new[] { "rRna", "mRna", "tRna" };
            _cb = GetComponent<CombatComponent>();
            _cb.Life = white.life;
            _cb.Damage = 10;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _target = new Target();
            white._infection.InfectionResist = 100;
            mov = new MovementUtility();
            _resourceManager = ResourceManager.Instance;
            dt = Time.time;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!turnTarget)
            {
                if (_target.isTargetValid(target, transform, white.sightRange))
                {
                    mov.MoveTowards(transform, target, _rb, white.moveSpeed, hasHit);
                    if (hasHit)
                    {
                        var rnaComponent = target.GetComponent<Rna>();
                        var l = rnaComponent.type;
                        switch (l)
                        {
                            case RnaType.rRna:
                                _resourceManager.rRna += 1;
                                break;
                            case RnaType.mRna:
                                _resourceManager.mRna += 1;
                                break;
                            case RnaType.tRna:
                                _resourceManager.tRna += 1;
                                break;
                            default:
                                Debug.Log("Dendritic Failed to get RNA");
                                break;
                        }

                        Destroy(target.gameObject);
                        target = null;
                    }
                    // if (hasHit && Time.time - dt > attackRate)
                    // {
                    //     white.AttackTarget(white.target, _cb.Damage);
                    //     dt = Time.time;
                    // }
                }
                else
                {
                    hasHit = false;
                    target = _target.GetTarget(white.detectionRange, overlapResults, transform, searchTag);
                }
            }
            else
            {
                if (!_cellDatabase.IsCellSpecial(turnCellId))
                {
                    if (_target.isTargetValid(turnCell))
                    {
                        mov.MoveTowards(transform, turnCell, _rb, white.moveSpeed, hasHit);
                        if (hasHit && Time.time - dt > attackRate)
                        {
                            _resourceManager.rRna -= 10;
                            dt = Time.time;
                            var turn_cell = turnCell.GetComponent<UtilityCells>();
                            turn_cell.turn = true;
                            turn_cell.newCell = transform;
                            ClearTurnTarget();
                        }
                    }
                    else
                    {
                        ClearTurnTarget();
                    }
                }
                else
                {
                    ClearTurnTarget();
                }
            }

            if (white._infection.infected)
            {
                white._infection.EndInfection();
            }
        }

        private void ClearTurnTarget()
        {
            turnCell = null;
            turnTarget = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (target != null && !turnTarget)
            {
                hasHit = other.transform == target;
                return;
            }

            if (!turnTarget) return;
            turnCellId = other.GetComponent<CellTypeIdentifier>().Type;
            hasHit = other.transform == turnTarget;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            hasHit = false;
        }
    }
}