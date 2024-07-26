using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fill;

        private int health;

        public int Health
        {
            get => health;
            set
            {
                health = value;
                _slider.value = health;
            }
        }

        public void SetMaxHealth(int value)
        {
            health = value;
            _slider.maxValue = health;
            _slider.value = health;
        }
    }
}