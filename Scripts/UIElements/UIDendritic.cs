using UnityEngine;

namespace UIElements
{
    public class UIDendritic : MonoBehaviour
    {
        public CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleUI();
            }
        }


        public void ToggleUI()
        {
            if (canvasGroup.alpha > 0)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }

        public void ShowUI()
        {
            canvasGroup.alpha = 1; // Visible
            canvasGroup.interactable = true; // Interactive
            canvasGroup.blocksRaycasts = true; // Can receive input
        }

        public void HideUI()
        {
            canvasGroup.alpha = 0; // Invisible
            canvasGroup.interactable = false; // Non-Interactive
            canvasGroup.blocksRaycasts = false; // Can't receive input
        }
    }
}