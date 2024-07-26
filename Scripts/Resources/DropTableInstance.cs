using UnityEngine;
using UnityEngine.SceneManagement;

namespace Resources
{
    public class DropTableInstance : MonoBehaviour
    {
        public static DropTableInstance Instance { get; private set; }
        [SerializeField] private DropsDatabase _drop;

        private bool isQuitting;
        private bool isSceneUnloading;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            Application.quitting += OnApplicationQuitting;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            Application.quitting -= OnApplicationQuitting;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnApplicationQuitting()
        {
            isQuitting = true;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            isSceneUnloading = true;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            isSceneUnloading = false;
        }

        public void SetDrop(Vector3 position, DropEnum type)
        {
            if (isQuitting || isSceneUnloading) return;
            var prefab = _drop.GetPrefab(type);
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}