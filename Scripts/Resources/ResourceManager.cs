using System;
using UnityEngine;


namespace Resources
{
    public enum RnaType
    {
        rRna,
        mRna,
        tRna
    }
    public class ResourceManager : MonoBehaviour
    {
        

        public static ResourceManager Instance { get; private set; }

        public event Action<RnaType, int> OnRnaChanged;
        [SerializeField] private int rrna;

        public int rRna
        {
            get => rrna;
            set
            {
                rrna = value;
                OnRnaChanged?.Invoke(RnaType.rRna, rrna);
            }
        }

        public int mrna;

        public int mRna
        {
            get => mrna;
            set
            {
                mrna = value;
                OnRnaChanged?.Invoke(RnaType.mRna, mrna);
            }
        }

        public int trna;

        public int tRna
        {
            get => trna;
            set
            {
                trna = value;
                OnRnaChanged?.Invoke(RnaType.tRna, trna);
            }
        }


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            
            Debug.Log("Resource Manager STARTED");
        }

        private void OnDestroy()
        {
            Debug.Log("Resource Manager Destroyed");
        }

        private void OnValidate()
        {
            rRna = rrna;
            mRna = mrna;
            tRna = trna;
        }
    }
}