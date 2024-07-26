using System.Linq;
using UnityEngine;

namespace Cells
{
    [CreateAssetMenu(fileName = "CellDatabase")]
    public class CellDatabase : ScriptableObject
    {
        [System.Serializable]
        public class Cell
        {
            public enumCellType type;
            public GameObject prefab;
        }

        public Cell[] mappings;

        public GameObject GetPrefab(enumCellType type)
        {
            return (from mapping in mappings where mapping.type == type select mapping.prefab).FirstOrDefault();
        }
        public bool IsCellSpecial(enumCellType cellType)
        {
            return cellType is enumCellType.TCell or enumCellType.BCell;
        }
    }
}