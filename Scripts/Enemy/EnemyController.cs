using Cells;
using Combat;
using PathFinder;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyController2D : MonoBehaviour
    {
        public float detectionRange = 5.0f;

        public float moveSpeed = 2.0f;

        private Rigidbody2D rb;

        private TargetService _objTargetService;
        private string[] searchTag = { "CellDef", "Cell" };
        private Collider2D[] overlapResults = new Collider2D[3];
        private Transform target;
        private bool targetCondition;
        private float sightRange;

        private MovementUtility mov;
        private bool hasHit;

        public int infectFactor = 100;
        public GameObject effectAntiBody;

        private CombatComponent _cb;
        private float dt;
        private float attackRate = 1f;

        private void Awake()
        {
            _cb = GetComponent<CombatComponent>();
            _cb.life = 50;
            _cb.Damage = 10;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _objTargetService = new TargetService();
            target = _objTargetService.FindTarget(detectionRange, overlapResults, transform, searchTag);
            sightRange = detectionRange;
            mov = new MovementUtility();
            dt = Time.time;
        }

        private void FixedUpdate()
        {
            if (_objTargetService.IsTargetValid(target, transform, sightRange))
            {
                mov.MoveTowards(transform, target, rb, moveSpeed, hasHit);
                if (hasHit && Time.time - dt > attackRate)
                {
                    AttackTarget(target, _cb.Damage);
                    dt = Time.time;
                }
            }
            else
            {
                mov.WanderRandomly(transform, rb, moveSpeed);
                hasHit = false;
                target = _objTargetService.FindTarget(detectionRange, overlapResults, transform, searchTag);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform == target)
            {
                var it = other.GetComponent<InfectionComponent>();
                hasHit = true;
                if (it == null) return;
                if (it.infected) return;
                float randomValue = Random.Range(0, infectFactor);
                if (!(randomValue > it.InfectionResist)) return;
                var enemyId = transform.GetComponent<EnemyVirusIdentifier>();
                it.virusType = enemyId.Type;
                it.StartInfection();
                Destroy(gameObject);
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
                hasHit = false;
            }
        }

        public void AttackTarget(Transform target, float amount)
        {
            var _cb_target = target.GetComponent<CombatComponent>();
            if (_cb_target != null)
            {
                _cb_target.TakeDamage(amount);
            }
        }
    }
}