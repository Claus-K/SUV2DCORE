using System;
using Combat;
using Unity.VisualScripting;
using UnityEngine;

namespace Cells
{
    public class ChildTriggerNotifier : MonoBehaviour
    {
        // public ParentTriggerHandler parentHandler;
        private CombatComponentV2 _cbv2;

        private void Start()
        {
            _cbv2 = GetComponentInParent<CombatComponentV2>();
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("CellDef"))
            {
                if (other.GetComponent<CellTypeIdentifier>().Type == enumCellType.Memory) return;
                other.GetComponent<CombatComponentV2>().ApplyFlatDamageBuff(_cbv2.Damage);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("CellDef"))
            {
                if (other.GetComponent<CellTypeIdentifier>().Type != enumCellType.Memory)
                {
                    other.GetComponent<CombatComponentV2>().RemoveFlatDamageBuff(_cbv2.Damage);
                }
            }
        }
    }
}