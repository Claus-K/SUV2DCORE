using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fill;
        public Gradient gradient;

        private int health;

        public int Health
        {
            get => health;
            set
            {
                health = value;
                _slider.value = health;
                _fill.color = gradient.Evaluate(_slider.normalizedValue);
            }
        }

        public void SetMaxHealth(int value)
        {
            health = value;
            _slider.maxValue = health;
            _slider.value = health;
            _fill.color = gradient.Evaluate(1f);
        }
    }
}