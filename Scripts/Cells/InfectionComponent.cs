using Unity.Mathematics;
using UnityEngine;

namespace Cells
{
    public class InfectionComponent : MonoBehaviour
    {
        public bool infected;
        public float infectionDuration = 10f;
        public GameObject enemyPrefab;
        private float infectionTime;
        private SpriteRenderer _spriteRenderer;
        private Color defaultEffect;
        private Color infectedEffect = Color.green;
        private int infectionSpawn;
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer)
            {
                defaultEffect = _spriteRenderer.color;
            }
            // scalable 
            infectionSpawn = 2;
        }

        public void StartInfection()
        {
            if (!infected)
            {
                infected = true;
                infectionTime = Time.time;

                if (!_spriteRenderer) return;
                _spriteRenderer.color = infectedEffect;
            }
        }

        public void EndInfection()
        {
            if (Time.time - infectionTime > infectionDuration)
            {
                infected = false;
                _spriteRenderer.color = defaultEffect;
                Destroy(gameObject);
                for (int i = 0; i < infectionSpawn; i++)
                {
                    Instantiate(enemyPrefab, transform.position, quaternion.identity);
                }
            }
        }
    }
}