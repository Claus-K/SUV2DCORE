using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CombatComponent : MonoBehaviour
    {
        public float Life { get; set; }
        private float damage;

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        private float maxLife;

        public Slider lifeBar;
        public Image lifeColor;
        private UnityEngine.Camera _camera;


        private void Start()
        {
            maxLife = Life;
            UpdateLifeBar();
            _camera = UnityEngine.Camera.main;
        }

        private void FixedUpdate()
        {
            lifeBar.transform.rotation = _camera.transform.rotation;
        }

        public void TakeDamage(float amount)
        {
            Life = Mathf.Max(Life - amount, 0);
            if (Life == 0)
            {
                Die();
            }

            UpdateLifeBar();
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        void UpdateLifeBar()
        {
            float lifePercentage = Life / maxLife;
            if (Life >= maxLife)
            {
                Life = maxLife;
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