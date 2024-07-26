using Unity.Mathematics;
using UnityEngine;

namespace Cells
{
    public class InfectionComponent : MonoBehaviour
    {
        public bool infected;
        public float infectionDuration = 3f;
        private GameObject enemyPrefab;
        private float infectionTime;
        private SpriteRenderer _spriteRenderer;
        private Color defaultEffect;
        private readonly Color infectedEffect = Color.yellow;
        
        private int infectionResist = 30;
        public int InfectionResist
        {
            get => infectionResist;
            set => infectionResist = value;
        }

        public GameObject infectedCellPrefab;
        public Enemy.enumVirusType virusType;

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer)
            {
                defaultEffect = _spriteRenderer.color;
            }

            // _baseCell = GetComponent<BaseCell>();
            // _baseCell.moveSpeed = Mathf.Floor(_baseCell.moveSpeed * 0.5f);
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
                Destroy(transform.gameObject);
                GameObject infectedCell = Instantiate(infectedCellPrefab, transform.position, quaternion.identity);
                infectedCell.GetComponent<InfectionSpawner>().enemyType = virusType;
            }
        }
    }
}