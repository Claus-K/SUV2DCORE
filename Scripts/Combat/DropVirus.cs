using UnityEngine;

namespace Combat
{
    public class DropVirus : MonoBehaviour
    {
        [SerializeField] private Resources.ResourceDatabaseRna _resource;
        private bool isSceneUnloading;

        private void OnDisable()
        {
            
            if (isSceneUnloading) return;
            var position = transform.position;

            var randomValue = Random.Range(0, 10);
            switch (randomValue)
            {
                case < 6: // 0-5 (6 values, 60% chance)
                    Instantiate(_resource.GetPrefab(Resources.resourceTypeRna.r), position, Quaternion.identity);
                    break;
                case < 9 and >= 6: // 6-8 (3 values, 30% chance)
                    Instantiate(_resource.GetPrefab(Resources.resourceTypeRna.m), position, Quaternion.identity);
                    break;
                case >= 9: // 9 (1 value, 10% chance)
                    Instantiate(_resource.GetPrefab(Resources.resourceTypeRna.t), position, Quaternion.identity);
                    break;
            }
        }

        private void OnApplicationQuit()
        {
            isSceneUnloading = true;
        }
    }
}