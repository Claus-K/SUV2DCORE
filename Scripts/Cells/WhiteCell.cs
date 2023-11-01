using Combat;
using PathFinder;
using UnityEngine;


namespace Cells
{
    public class WhiteCell : BaseCell
    {
        // movement variables
        public Rigidbody2D _rb;
        public string[] searchTag = { "Virus", "Bacteria", "Parasite" };

        public MovementUtility mov = new();

        // target variables
        public Collider2D[] overlapResults = new Collider2D[3];
        public Target _target = new();
        public Transform target;

        private UtilityCells _utilityCells;

        public WhiteCell(float detectionRange, float sightRange, float moveSpeed, float life = 50) : base(
            detectionRange, sightRange,
            moveSpeed, life)
        {
        }

        public void AttackTarget(Transform target, float amount)
        {
            if (target == null)
            {
                Debug.Log("Target is NULL");
                return;
            }

            var _cb = target.GetComponent<CombatComponent>();
            if (_cb != null)
            {
                _cb.TakeDamage(amount);
            }
        }
    }
}