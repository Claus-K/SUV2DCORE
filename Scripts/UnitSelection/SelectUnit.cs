using System.Collections.Generic;
using UnityEngine;

namespace UnitSelection
{
    public class SelectUnit : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        // private GameObject _selectedUnit;

        private List<GameObject> _selectedUnits = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            _camera = UnityEngine.Camera.main;

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))  // 0 is the left mouse button
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                // If we hit something
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    GameObject clickedUnit = hit.collider.gameObject;

                    // If Shift/Ctrl is not held down, clear current selection
                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                    {
                        DeselectAllUnits();
                    }

                    // Toggle the selection status of the clicked unit
                    if (_selectedUnits.Contains(clickedUnit))
                    {
                        _selectedUnits.Remove(clickedUnit);
                        clickedUnit.GetComponent<SpriteRenderer>().color = Color.white; // revert color
                    }
                    else
                    {
                        _selectedUnits.Add(clickedUnit);
                        clickedUnit.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
                // If we did not hit any unit
                else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    DeselectAllUnits();
                }
            }
        }
        
        void DeselectAllUnits()
        {
            foreach (GameObject unit in _selectedUnits)
            {
                unit.GetComponent<SpriteRenderer>().color = Color.white;
            }
            _selectedUnits.Clear();
        }

    }
}
