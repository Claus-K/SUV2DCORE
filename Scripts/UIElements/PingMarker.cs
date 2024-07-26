using UnityEngine;

namespace UIElements
{
    public class PingScript : MonoBehaviour
    {
        [SerializeField] private float duration;

        private float dt;

        
        private void Start()
        {
            dt = Time.time;
        }


        private void Update()
        {
            if (Time.time - dt > duration)
            {
                Destroy(gameObject);
            }
        }
    }
}