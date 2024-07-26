using System.Collections;
using Enemy;
using UnityEngine;

namespace Cells
{
    public class InfectionComponentV2 : MonoBehaviour
    {
        [SerializeField] private int InfectionResits;
        [SerializeField] private float InfectionDuration;
        public bool debugInfected;
        public bool Infected { get; set; }
        public int InfRes => InfectionResits;
        // private float dt;

        [SerializeField] private GameObject InfectedCellPrefab;
        public enumVirusType virusType;

        private void Start()
        {
            if (debugInfected)
            {
                StartInfection(enumVirusType.A);
            }
        }

        public void StartInfection(enumVirusType type)
        {
            if (!Infected)
            {
                virusType = type;
                Infected = true;
                StartCoroutine(StartInfectionCoroutine());
            }
        }

        private IEnumerator StartInfectionCoroutine()
        {
            var sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            if (sprite) sprite.color = Color.green;
            yield return new WaitForSeconds(3.5f);
            EndInfection();
        }

        public void EndInfection()
        {
            var infectedCell = Instantiate(InfectedCellPrefab, transform.position, Quaternion.identity);
            infectedCell.GetComponent<InfectionSpawner>().enemyType = virusType;
            Destroy(gameObject);
        }
    }
}