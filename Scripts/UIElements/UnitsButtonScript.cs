using System;
using Cells;
using UnitSelection;
using UnityEngine;

namespace UIElements
{
    public class UnitsButtonScript : MonoBehaviour
    {
        public void SelectUnitsByType(string type)
        {
            // Debug.Log($"Clicking on type --> {type}");

            SelectDictionary.Instance.SetDictionaryFromDictionary(
                ScenePoints.Instance.CellsList[StringToEnumCellType(type)], Input.GetKey(KeyCode.LeftShift));
        }

        private enumCellType StringToEnumCellType(string value)
        {
            Enum.TryParse(value, out enumCellType type);
            // Debug.Log($"Parse :: {type}");
            return type;
        }
    }
}