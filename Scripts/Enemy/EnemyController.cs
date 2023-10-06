using Cells;
using PathFinder;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyController2D : MonoBehaviour
    {
        public float detectionRange = 5.0f;

        public float moveSpeed = 2.0f;

        private Rigidbody2D rb;
        private float dt;
        public enumVirusType A;

        private Target objTarget;
        private string searchTag;
        private Collider2D[] overlapResults = new Collider2D[3];
        private Transform target;
        private bool targetCondition;
        private float sightRange;

        private MovementUtility mov;
        private bool hitCollider;

        public int infectFactor = 100;
        public GameObject effectAntiBody;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            objTarget = new Target();
            searchTag = "CellDef";
            target = objTarget.GetTarget(detectionRange, overlapResults, transform, searchTag);
            sightRange = detectionRange;
            dt = Time.time;
            mov = new MovementUtility();
        }

        private void FixedUpdate()
        {
            if (objTarget.isTargetValid(target, transform, sightRange))
            {
                mov.MoveTowards(transform, target, rb, moveSpeed, hitCollider);
            }
            else
            {
                mov.WanderRandomly(transform, rb, moveSpeed);
                hitCollider = false;
                target = objTarget.GetTarget(detectionRange, overlapResults, transform, searchTag);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform == target)
            {
                var it = other.GetComponent<InfectionComponent>();
                hitCollider = true;
                if (it == null) return;
                if (it.infected) return;
                float randomValue = Random.Range(0, infectFactor);
                if (!(randomValue > it.infectionResist)) return;
                var enemyId = transform.GetComponent<EnemyVirusIdentifier>();
                it.virusType = enemyId.Type;
                it.StartInfection();
            }

            if (other.transform.CompareTag("AntiBody"))
            {
                effectAntiBody.SetActive(true);
                // Debug.Log("Infect Factor active - 10f : " + infectFactor);
                infectFactor = Mathf.Max(infectFactor - 10, 0);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == target)
            {
                hitCollider = false;
            }
        }
    }
}