using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class ButtonHover : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private Color hoverColor;

        private Color originalColor;

        private ColorBlock cb;

        private void Start()
        {
            cb = _button.colors;
            originalColor = cb.normalColor;
            hoverColor = cb.highlightedColor;
        }

        public void HoverEnter()
        {
            cb.selectedColor = hoverColor;
            _button.colors = cb;
        }

        public void HoverExit()
        {
            cb.selectedColor = originalColor;
            _button.colors = cb;
        }

        public void ClickDebug()
        {
            Debug.Log("Click");
            // Debug.Log($"I was Clicked --> {transform.name}");
        }
    }
}