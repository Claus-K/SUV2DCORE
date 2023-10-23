using System.Linq;
using Combat;
using PathFinder;
using TMPro;
using UIElements;
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

        private Transform turnTarget;
        private CombatComponent _cb;
        private int rnaPoints;
        public TMP_InputField rnaInfo;
        public RnaHandler _rnaHandler;


        private void Awake()
        {
            white = new WhiteCell(6, 6, 2, 150f)
            {
                _infection = GetComponent<InfectionComponent>()
            };
            searchTag = white.searchTag.Concat(new[] { "VirusCorpse" }).ToArray();

            _cb = GetComponent<CombatComponent>();
            _cb.Life = white.life;
            _cb.Damage = 10;

            _rnaHandler = rnaInfo.GetComponent<RnaHandler>();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _target = new Target();
            white._infection.InfectionResist = 100;
            dt = Time.time;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (turnTarget == null)
            {
                if (_target.isTargetValid(target, transform, white.sightRange))
                {
                    mov.MoveTowards(transform, target, _rb, white.moveSpeed, hasHit);
                    if (hasHit && Time.time - dt > attackRate)
                    {
                        white.AttackTarget(white.target, _cb.Damage);
                        dt = Time.time;
                    }
                }
                else
                {
                    hasHit = false;
                    target = _target.GetTarget(white.detectionRange, overlapResults, transform, searchTag);
                }
            }
            else
            {
                if (_target.isTargetValid(turnTarget))
                {
                    mov.MoveTowards(transform, turnTarget, _rb, white.moveSpeed, hasHit);
                    if (hasHit && Time.time - dt > attackRate)
                    {
                        _rnaHandler.SpendRna(10);
                        dt = Time.time;
                        // TODO turnTarget = null;
                    }
                }
            }

            if (white._infection.infected)
            {
                white._infection.EndInfection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (target != null && turnTarget == null)
            {
                hasHit = other.transform == target;
                return;
            }

            if (turnTarget != null)
            {
                hasHit = other.transform == turnTarget;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            hasHit = false;
        }
    }
}