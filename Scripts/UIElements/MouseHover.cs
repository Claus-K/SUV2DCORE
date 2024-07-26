using System;
using System.Collections;
using Combat;
using UnityEngine;
using UnityEngine.Serialization;

namespace UIElements
{
    public class MouseHover : MonoBehaviour
    {
        [SerializeField] private string _name;
        private bool isHovering;

        private void OnMouseEnter()
        {
            StartCoroutine(HoverInfor());
        }

        private void OnMouseExit()
        {
            isHovering = false;
            UnitInfor.Instance.DisableUnitInfor();
        }

        private void OnDestroy()
        {
            if (!isHovering) return;
            isHovering = false;
            UnitInfor.Instance.DisableUnitInfor();
        }


        private IEnumerator HoverInfor()
        {
            if (isHovering) yield break;
            isHovering = true;
            var cb = GetComponent<CombatComponentV2>();
            while (isHovering)
            {
                cb.GetHealth(out int health, out int maxHealth);
                UnitInfor.Instance.ShowUnitInfor(_name, health, maxHealth);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}