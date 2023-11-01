using Enemy;
using UnityEngine;

namespace Cells
{
    public class UtilityCells : MonoBehaviour
    {
        public bool turn;
        public Transform newCell;
        public void TurnCell(Transform ownCell)
        {
            Destroy(ownCell);
            Instantiate(newCell);
        }

        private enumVirusType InitCell(enumVirusType type)
        {
            return type;
        }
    }
}
