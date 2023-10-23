using Unity.Mathematics;
using UnityEngine;

namespace Cells
{
    public class InfectionSpawner : MonoBehaviour
    {
        [SerializeField] private Enemy.EnemyVirusDatabase _enemyVirusDatabase;
        private float spawnRate = 10f;
        private float dt;
        public Enemy.enumVirusType enemyType;
        private GameObject enemyPrefab;

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
            }
        }

        private void SpawnEnemy()
        {
            var spawn = Instantiate(enemyPrefab, transform.position, quaternion.identity);
            var rb = spawn.GetComponent<Rigidbody2D>();
            rb.AddForce(spawn.transform.up * 2, ForceMode2D.Impulse);
        }
    }
}