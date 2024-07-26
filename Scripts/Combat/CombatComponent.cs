using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CombatComponent : MonoBehaviour
    {
        [SerializeField]
        public float life;

        

        public float Damage { get; set; }

        private float maxLife;

        public Slider lifeBar;
        public Image lifeColor;
        private UnityEngine.Camera _camera;


        private void Start()
        {
            maxLife = life;
            UpdateLifeBar();
            // _camera = UnityEngine.Camera.main;
        }

        // private void FixedUpdate()
        // {
        //     lifeBar.transform.rotation = _camera.transform.rotation;
        // }

        public void TakeDamage(float amount)
        {
            life = Mathf.Max(life - amount, 0);
            if (life == 0)
            {
                Die();
            }
            // Debug.Log($"{life}");
            UpdateLifeBar();
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void UpdateLifeBar()
        {
            var lifePercentage = life / maxLife;
            if (life >= maxLife)
            {
                life = maxLife;
                lifeBar.transform.gameObject.SetActive(false);
            }
            else
            {
                lifeBar.transform.gameObject.SetActive(true);
            }

            if (lifePercentage > 0.70)
            {
                lifeColor.color = Color.green;
            }

            if (lifePercentage < 0.70)
            {
                lifeColor.color = Color.yellow;
            }
            else
            {
                if (lifePercentage < 0.30)
                {
                    lifeColor.color = Color.red;
                }
            }

            lifeBar.value = Mathf.Max(lifePercentage, 0);
        }
    }
    
    
}