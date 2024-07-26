using Combat;
using Enemy;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cells
{
    public class BCellProjectile : MonoBehaviour
    {
        [SerializeField]
        private float lifeTime;
        private float dt;

        [SerializeField] private int Damage;
        private bool hitFlag;

        

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
            if (other.transform.CompareTag("Virus") && !hitFlag)
            {
                var _cb = other.gameObject.GetComponent<CombatComponentV2>();
                if (_cb != null)
                {
                    _cb.TakeDamage(Damage);
                }
                var dotHandle = other.gameObject.GetComponent<DOTHandler>();
                if (dotHandle != null)
                {
                    dotHandle.ApplyAntiBody(3);
                }
                
                
                Destroy(transform.gameObject);
                hitFlag = true;
            }
        }
    }
}