using UnityEngine;
using TMPro;

namespace UIElements
{
    public class RnaHandler : MonoBehaviour
    {
        public int rna;

        public int Rna
        {
            get => rna;
            set => rna = value;
        }
        
        

        public TMP_InputField input;
        private string info;
        void Start()
        {
            info = input.text;
        }
        
        void FixedUpdate()
        {
            input.text = rna.ToString();
        }

        public bool canUse(int cost)
        {
            return Rna - cost >= 0;
        }

        public void SpendRna(int cost)
        {
            if (canUse(cost))
            {
                Rna -= cost;
            }
            
        }
    }
}
