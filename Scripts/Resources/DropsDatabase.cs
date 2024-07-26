using System.Linq;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(fileName = "DropDatabase")]
    public class DropsDatabase : ScriptableObject
    {
        [System.Serializable]
        public class DropTableMapping
        {
            public DropEnum type;
            public GameObject prefab;
        }

        public DropTableMapping[] mappings;

        public GameObject GetPrefab(DropEnum type)
        {
            return (from mapping in mappings where mapping.type == type select mapping.prefab).FirstOrDefault();
        }

    }
}
