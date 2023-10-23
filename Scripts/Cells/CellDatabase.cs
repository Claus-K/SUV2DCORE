using System.Collections;
using System.Collections.Generic;
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
        }

        public Cell[] cells;

        public bool IsCellSpecial(enumCellType cellType)
        {
            return cellType is enumCellType.TCell or enumCellType.BCell;
        }
    }
}