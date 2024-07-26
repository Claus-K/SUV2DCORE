using System.Linq;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(fileName = "ResourceDatabaseRna")]
    public class ResourceDatabaseRna : ScriptableObject
    {
        [System.Serializable]
        public class ResourceRnaMapping
        {
            public resourceTypeRna type;
            public GameObject prefab;
        }

        public ResourceRnaMapping[] mappings;

        public GameObject GetPrefab(resourceTypeRna type)
        {
            return (from mapping in mappings where mapping.type == type select mapping.prefab).FirstOrDefault();
        }
    }
}