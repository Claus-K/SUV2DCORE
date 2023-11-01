using System;
using Resources;
using UnityEngine;

namespace PathFinder
{
    public class CollectComponent : MonoBehaviour
    {
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public RnaType? getRna(string tag)
        {
            if (Enum.TryParse(tag, true, out RnaType type))
            {
                return type;
            }
            return null;
        }
        
        
    }
}
