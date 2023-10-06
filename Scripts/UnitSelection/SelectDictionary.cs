using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitSelection
{
    public class SelectDictionary : MonoBehaviour
    {
        public static SelectDictionary Instance { get; private set; }

        public Dictionary<int, GameObject> selectUnits = new Dictionary<int, GameObject>();
        // Start is called before the first frame update

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddSelected(GameObject go)
        {
            int id = go.GetInstanceID();

            if (selectUnits.ContainsKey(id)) return;
            selectUnits.Add(id, go);
            // add effect of selected here
            go.GetComponent<SpriteRenderer>().color = Color.red;
            Debug.Log("Added " + id + " to selected units.");
        }

        public void Deselect(int id)
        {
            selectUnits.Remove(id);
        }

        public void DeselectAll()
        {
            foreach (var pair in selectUnits.Where(pair => pair.Value != null))
            {
                // Return select to normal here
                selectUnits[pair.Key].GetComponent<SpriteRenderer>().color = Color.white;
            }

            selectUnits.Clear();
        }

        
    }
}