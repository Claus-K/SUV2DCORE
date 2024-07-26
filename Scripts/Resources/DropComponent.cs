using UnityEngine;

namespace Resources
{
    public class DropComponent : MonoBehaviour
    {
        [SerializeField] private DropEnum type;

        private void OnDestroy()
        {
            var chance = Random.Range(0, 100);
            if (chance < 30)
            {
                DropTableInstance.Instance?.SetDrop(transform.position, type);
            }
            
        }
    }
}