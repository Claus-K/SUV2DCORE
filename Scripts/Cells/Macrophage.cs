using UnityEngine;

namespace Cells
{
    public class Macrophage : MonoBehaviour
    {
        private WhiteCell white;

        // Start is called before the first frame update
        private float dt;
        private bool hitCollider;

        // macrophage variables
        private int enemiesEaten;
        private int max_enemies_eaten = 20;

        private void Awake()
        {
            white = new WhiteCell(10, 10, 2, 250f)
            {
                _infection = GetComponent<InfectionComponent>(),
                _rb = GetComponent<Rigidbody2D>()
            };
            white.damage = 20;
        }

        private void Start()
        {
            // if resist not informed is set to 30
            white._infection.InfectionResist = 80;
            dt = Time.time;
        }

        private void FixedUpdate()
        {
            if (white._target.isTargetValid(white.target, transform, white.sightRange))
            {
                white.mov.MoveTowards(transform, white.target, white._rb, white.moveSpeed, hitCollider);
                // is Target valid
                // if (!objTarget.Reevaluate(Time.time, dt, reevaluationTime)) return;
                // core.target = null;
                // dt = Time.time;
            }
            else
            {
                hitCollider = false;
                white.target =
                    white._target.GetTarget(white.detectionRange, white.overlapResults, transform, white.searchTag);
            }

            if (white._infection.infected)
            {
                white._infection.EndInfection();
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

            if (white.target == null) return;
            hitCollider = other.transform == white.target;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == white.target)
            {
                hitCollider = false;
            }
        }
    }
}