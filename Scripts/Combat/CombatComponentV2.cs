using System.Collections;
using UIElements;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CombatComponentV2 : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private Slider _healthBarSlider;
        private HealthBar _healthBar;
        private int timeoutHealthBar;

        [SerializeField] private int damage;
        private int damageBuff;
        private float percetangeBuff = 1;

        public int Damage
        {
            get => (int)(damage + damageBuff * percetangeBuff);
            set => damage = value;
        }

        private void Start()
        {
            _healthBar = _healthBarSlider.GetComponent<HealthBar>();
            _healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int value)
        {
            timeoutHealthBar = 4;
            if (!_healthBarSlider.gameObject.activeSelf)
            {
                StartCoroutine(ShowHealth());
            }

            if (!_healthBar) return;

            _healthBar.Health -= value;
            _healthBar.Health = Mathf.Clamp(_healthBar.Health, 0, maxHealth);
            if (_healthBar.Health == 0)
            {
                Kill();
            }
        }

        private IEnumerator ShowHealth()
        {
            if (_healthBarSlider.gameObject.activeSelf) yield break;
            _healthBarSlider.gameObject.SetActive(true);
            while (timeoutHealthBar > 0)
            {
                timeoutHealthBar -= 1;
                yield return new WaitForSeconds(1f);
            }

            _healthBarSlider.gameObject.SetActive(false);
        }

        public void GetHealth(out int currentHealth, out int maxHealthValue)
        {
            currentHealth = _healthBar.Health;
            maxHealthValue = maxHealth;
        }

        public void ApplyFlatDamageBuff(int value)
        {
            damageBuff += value;
        }

        public void ApplyPorcDamageBuff(float value)
        {
            percetangeBuff += value;
        }

        public void RemoveFlatDamageBuff(int value)
        {
            damageBuff = Mathf.Max(damageBuff - value, 0);
        }

        public void RemovePorcDamageBuff(float value)
        {
            percetangeBuff = Mathf.Max(percetangeBuff - value, 1f);
        }

        private void Kill()
        {
            Destroy(gameObject);
        }
    }
}