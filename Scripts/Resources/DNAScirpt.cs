using System;
using Cells;
using UnityEngine;

namespace Resources
{
    public class DNAScirpt : MonoBehaviour
    {
        [SerializeField] private int points;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("CellDef"))
            {
                var cellTypeIdentifier = other.GetComponent<CellTypeIdentifier>();
                if (cellTypeIdentifier && cellTypeIdentifier.Type == enumCellType.Dendritic)
                {
                    ResourceManagerV2.Instance.Energy += points;
                    Destroy(gameObject);
                }
            }
        }
    }
}