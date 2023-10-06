using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Cells
{
    public class BCellProjectile : MonoBehaviour
    {
        private float lifeTime = 5f;
        private float dt;
        
        private void Start()
        {
            dt = Time.time;
        }

        private void FixedUpdate()
        {
            if (Time.time - dt > lifeTime)
            {
                Destroy(transform.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Virus"))
            {
                Destroy(transform.gameObject);
            }
        }
    }
}