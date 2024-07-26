using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Unity.VisualScripting;
using UnityEngine;

namespace UnitSelection
{
    public class SelectDictionary : MonoBehaviour
    {
        public static SelectDictionary Instance { get; private set; }

        public Dictionary<int, GameObject> selectUnits = new();
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
            var id = go.GetInstanceID();

            if (selectUnits.ContainsKey(id)) return;
            selectUnits.Add(id, go);
            // add effect of selected here
            go.GetComponent<SelectionComponent>().EnableSelection();
            // go.GetComponent<SelectionComponent>().selectionCircle.SetActive(true);
            // Debug.Log("Added " + id + " to selected units.");
        }

        private void AddSelected(int id, GameObject go)
        {
            if (!selectUnits.TryAdd(id, go)) return;
            go.GetComponent<SelectionComponent>().EnableSelection();
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
                // selectUnits[pair.Key].GetComponent<SpriteRenderer>().color = Color.white;
                // selectUnits[pair.Key].GetComponent<SelectionComponent>().selectionCircle.SetActive(false);
                selectUnits[pair.Key].GetComponent<SelectionComponent>().DisableSelection();
            }

            selectUnits.Clear();
        }

        private void SetUnitDictionary(Dictionary<int, GameObject> dictionary)
        {
            var keys = dictionary.Keys;
            foreach (var key in keys)
            {
                AddSelected(key, dictionary[key]);
            }
        }

        private Dictionary<int, GameObject> UnitsMerge(Dictionary<int, GameObject> newDictionary,
            Dictionary<int, GameObject> previousDicionary)
        {
            var keys = previousDicionary.Keys;
            foreach (var key in keys.Where(key => !newDictionary.ContainsKey(key)))
            {
                newDictionary.Add(key, previousDicionary[key]);
            }

            return newDictionary;
        }

        public void SetDictionaryFromDictionary(Dictionary<int, GameObject> dictionary, bool merge = false)
        {
            if (!merge)
            {
                SetUnitDictionary(dictionary);
            }
            else
            {
                var currentDict = new Dictionary<int, GameObject>(selectUnits);
                SetUnitDictionary(dictionary);
                var newDict = new Dictionary<int, GameObject>(selectUnits);
                selectUnits = UnitsMerge(newDict, currentDict);
            }
        }

        public Dictionary<int, GameObject> GetDictionary()
        {
            var keys = selectUnits.Keys.ToList();
            var keysToRemove = keys.Where(key => selectUnits[key] == null).ToList();
            foreach (var key in keysToRemove)
            {
                selectUnits.Remove(key);
            }
            return selectUnits;
        }
    }
}