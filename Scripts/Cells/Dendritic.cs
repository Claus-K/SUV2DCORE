using PathFinder;
using UnityEngine;

namespace Cells
{
    public class Dendritic : MonoBehaviour
    {
        private BaseCell core;

        private Collider2D[] overlapResults = new Collider2D[3];
        private string[] searchTag = { "Virus", "Bacteria", "Parasite" };

        private Rigidbody2D _rb;
        private Target _target;
        private Transform target;

        private bool hasHit;

        private MovementUtility mov;


        private void Awake()
        {
            core = new BaseCell(6, 6, 2, 150f)
            {
                _infection = GetComponent<InfectionComponent>()
            };
        }

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _target = new Target();
            core._infection.InfectionResist = 60;
        }

        // Update is called once per frame
        void Update()
        {
            if (_target.isTargetValid(target, transform, core.sightRange))
            {
                mov.MoveTowards(transform, target, _rb, core.moveSpeed, hasHit);
            }
            else
            {
                hasHit = false;
                target = _target.GetTarget(core.detectionRange, overlapResults, transform, searchTag);
            }

            if (core._infection.infected)
            {
                core._infection.EndInfection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (target != null)
            {
                hasHit = other.transform == target;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            hasHit = false;
        }
    }
}