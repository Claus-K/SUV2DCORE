using Combat;
using UnityEngine;

namespace Cells
{
    public class TCell : MonoBehaviour
    {
        public WhiteCell white;
        public float attackRate = 1f;
        private float dt;
        private bool hasHit;
        private CombatComponent _cb;

        private void Awake()
        {
            white = new WhiteCell(10, 10, 2, 150f)
            {
                _infection = GetComponent<InfectionComponent>(),
                _rb = GetComponent<Rigidbody2D>()
            };

            _cb = GetComponent<CombatComponent>();
            _cb.Life = white.life;
            _cb.Damage = 10;
        }

        void Start()
        {
            // if resist not informed is set to 30
            white._infection.InfectionResist = 60;
            dt = Time.time;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (white._target.isTargetValid(white.target, transform, white.detectionRange))
            {
                white.mov.MoveTowards(transform, white.target, white._rb, white.moveSpeed, hasHit);
                if (hasHit && Time.time - dt > attackRate)
                {
                    white.AttackTarget(white.target, _cb.Damage);
                    dt = Time.time;
                }
            }
            else
            {
                hasHit = false;
                white.target =
                    white._target.GetTarget(white.detectionRange, white.overlapResults, transform, white.searchTag);
            }

            if (white._infection.infected)
            {
                white._infection.EndInfection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (white.target != null)
            {
                hasHit = other.transform == white.target;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            hasHit = false;
        }
    }
}