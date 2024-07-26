using Combat;
using UnityEngine;

namespace Cells
{
    public class Engulf : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Virus"))
            {
                var combatComponent = other.GetComponent<CombatComponentV2>();
                if (combatComponent)
                {
                    combatComponent.TakeDamage(1000);
                }
            }
        }
    }
}
