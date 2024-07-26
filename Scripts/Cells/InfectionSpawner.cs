using Enemy;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Cells
{
    public class InfectionSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyVirusDatabase _enemyVirusDatabase;
        private float spawnRate = 0.5f;
        private float dt;
        public enumVirusType enemyType;
        private GameObject enemyPrefab;
        private int amount = 3;

        private void Start()
        {
            dt = Time.time;

            enemyPrefab = _enemyVirusDatabase.GetPrefab(enemyType);
            if (enemyPrefab == null)
            {
                Debug.LogError("No Prefab found : " + enemyType);
                Destroy(gameObject);
            }

            SpawnEnemy();
        }

        private void FixedUpdate()
        {
            if (Time.time - dt > spawnRate)
            {
                SpawnEnemy();
                dt = Time.time;
                amount -= 1;
                if (amount == 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void SpawnEnemy()
        {
            Instantiate(enemyPrefab, transform.position, quaternion.identity);
            // var rb = spawn.GetComponent<Rigidbody2D>();
            // rb.AddForce(spawn.transform.up * 2, ForceMode2D.Impulse);
        }
    }
}