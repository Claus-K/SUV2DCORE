using System.Linq;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyVirusDatabase")]
    public class EnemyVirusDatabase : ScriptableObject
    {
        [System.Serializable]
        public class EnemyPrefabMapping
        {
            public enumVirusType type;
            public GameObject prefab;
        }

        public EnemyPrefabMapping[] mappings;

        public GameObject GetPrefab(enumVirusType type)
        {
            return (from mapping in mappings where mapping.type == type select mapping.prefab).FirstOrDefault();
        }
    }
}
