using System.Collections.Generic;
using Cells;
using UnityEngine;

namespace UnitSelection
{
    public class SelectUnit : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        void Start()
        {
            _camera = UnityEngine.Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    GameObject clickedUnit = hit.collider.gameObject;
                    int id = clickedUnit.GetInstanceID();

                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                    {
                        SelectDictionary.Instance.DeselectAll();
                    }

                    if (SelectDictionary.Instance.selectUnits.ContainsKey(id))
                    {
                        SelectDictionary.Instance.Deselect(id);
                    }
                    else
                    {
                        SelectDictionary.Instance.AddSelected(clickedUnit);
                    }
                }
                else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    SelectDictionary.Instance.DeselectAll();
                }
            }
        }
    }
}