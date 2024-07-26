using UnityEngine;

namespace UIElements
{
    public class BillBoard : MonoBehaviour
    {
        private Transform _cam;

        private void Start()
        {
            _cam = UnityEngine.Camera.main?.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _cam.forward);
        }
    }
}