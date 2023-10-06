using PathFinder;
using UnityEngine;

namespace Cells
{
    public class Macrophage : MonoBehaviour
    {
        // Start is called before the first frame update
        public float detectionRange = 5.0f;
        public float moveSpeed = 2.0f;

        private Rigidbody2D rb;
        private float dt;
        private float sightRange;

        public InfectionComponent infection;

        private Target objTarget;
        private string searchTag;
        private Collider2D[] overlapResults = new Collider2D[3];
        private Transform target;

        private MovementUtility mov;
        private bool hitCollider;

        // macrophage variables
        private int enemiesEaten;
        private int max_enemies_eaten = 20;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            infection = GetComponent<InfectionComponent>();
            // if resist not informed default value is 30
            infection.infectionResist = 75;
            objTarget = new PathFinder.Target();
            searchTag = "Virus";
            target = objTarget.GetTarget(detectionRange, overlapResults, transform, searchTag);
            sightRange = detectionRange;
            mov = new MovementUtility();
        }

        private void FixedUpdate()
        {
            if (objTarget.isTargetValid(target, transform, sightRange))
            {
                mov.MoveTowards(transform, target, rb, moveSpeed, hitCollider);
                // is Target valid
                // if (!objTarget.Reevaluate(Time.time, dt, reevaluationTime)) return;
                // target = null;
                // dt = Time.time;
            }
            else
            {
                hitCollider = false;
                target = objTarget.GetTarget(detectionRange, overlapResults, transform, searchTag);
            }

            if (infection.infected)
            {
                infection.EndInfection();
            }

            if (enemiesEaten > 0)
            {
                if (Time.time - dt > 3f)
                {
                    Debug.Log("Before Max: " + enemiesEaten);
                    enemiesEaten = Mathf.Max(enemiesEaten - 1, 0);
                    Debug.Log("After Max: " + enemiesEaten);
                    dt = Time.time;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Virus"))
            {
                if (enemiesEaten < max_enemies_eaten)
                {
                    Destroy(other.transform.gameObject);
                    Debug.Log("Before ++ : " + enemiesEaten);
                    enemiesEaten = enemiesEaten + 1;
                    Debug.Log("After ++ : " + enemiesEaten);
                    dt = Time.time;
                }
            }

            if (target == null) return;
            hitCollider = other.transform == target;
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