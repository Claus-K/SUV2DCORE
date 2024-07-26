using PathFinder;
using UnityEngine;


namespace Cells
{
    public class WhiteCell : BaseCell
    {
        // movement variables
        public Rigidbody2D _rbody2D;
        public string[] searchTag = { "Virus", "Bacteria", "Parasite" };

        public MovementUtility movmentUtilitys = new();
        public PathAgent pathAgent = new();

        // target variables
        public Collider2D[] overlapResults = new Collider2D[50];
        public TargetService TargetServiceScript = new();
        public Transform target;

        private UtilityCells _utilityCells;

        public WhiteCell(float detectionRange, float sightRange, float moveSpeed, float life = 50) : base(
            detectionRange, sightRange,
            moveSpeed, life)
        {
        }
    }
}