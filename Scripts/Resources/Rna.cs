using Cells;
using UnityEngine;

namespace Resources
{
    public class Rna : MonoBehaviour
    {
        public RnaType type;
        private ResourceManager _rm;
        
        
        private void Start()
        {
            _rm = ResourceManager.Instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!GetComponent<Collider2D>().enabled) return;
            var ci = other.GetComponent<CellTypeIdentifier>();
            if (ci == null || ci.Type != enumCellType.Dendritic) return;
            GetComponent<Collider2D>().enabled = false;
            // Debug.Log($"Frame: {Time.frameCount}, Time: {Time.time}, Object: {this.gameObject.name}, Collided With: {other.gameObject.name}, Type: {ci.Type}");
            switch (type)
            {
                case RnaType.rRna:
                    _rm.rRna += 1;
                    break;
                case RnaType.mRna:
                    _rm.mRna += 1;
                    break;
                case RnaType.tRna:
                    _rm.tRna += 1;
                    break;
                default:
                    Debug.Log("Dendritic Failed to get RNA");
                    break;
            }
            Destroy(transform.gameObject);
        }
    }
}