using System;
using TMPro;
using UnityEngine;


namespace Resources
{
    public class RnaDisplay : MonoBehaviour
    {
        public TMP_InputField rRnaText;
        public TMP_InputField mRnaText;
        public TMP_InputField tRnaText;
        private ResourceManager _resourceManager;

        private void Awake()
        {
            _resourceManager = ResourceManager.Instance;
            _resourceManager.OnRnaChanged += UpdateRnaDisplay;
        }

        // private void Start()
        // {
        //     _resourceManager = ResourceManager.Instance;
        //     _resourceManager.OnRnaChanged += UpdateRnaDisplay;
        // }

        // Update is called once per frame
        private void OnDestroy()
        {
            if (_resourceManager != null)
            {
                _resourceManager.OnRnaChanged -= UpdateRnaDisplay;
            }
            
        }

        private void UpdateRnaDisplay(RnaType type, int value)
        {
            switch (type)
            {
                case RnaType.rRna:
                    rRnaText.text = "RNA: " + value.ToString();
                    break;
                case RnaType.mRna:
                    mRnaText.text = "mRna: " + value.ToString();
                    break;
                case RnaType.tRna:
                    tRnaText.text = "tRna: " + value.ToString();
                    break;
                default:
                    Debug.Log("Rna TYPE FAILED");
                    break;
            }
            
        }
    }
}
