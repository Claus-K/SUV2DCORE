using UnityEngine;

namespace PathFinder
{
    public class GridInit : MonoBehaviour
    {
        public GridScript gridScript;
        private UnityEngine.Camera _camera;


        // Start is called before the first frame update
        void Start()
        {
            _camera = UnityEngine.Camera.main;

            if (gridScript != null)
            {
                gridScript.Initialize(10, 10, 1f);
                Debug.Log("Grid is initialized");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!Input.GetMouseButton(0)) return;
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            gridScript.SetValue(mouseWorldPosition, 56);
        }
    }
}