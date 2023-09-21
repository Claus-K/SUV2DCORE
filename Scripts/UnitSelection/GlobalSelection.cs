using Unity.VisualScripting;
using UnityEngine;

namespace UnitSelection
{
    public class GlobalSelection : MonoBehaviour
    {
        private bool isSelecting = false;
        private Vector3 startMousePosition;
        public LayerMask unitLayerMask; // Assign this in the inspector to select only units.
        public RectTransform selectionBox; // UI element for the selection box (optional)

        private UnityEngine.Camera _camera;

        void Start()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSelecting = true;
                startMousePosition = Input.mousePosition;

                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                {
                    DeselectAllUnits();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isSelecting = false;

                if (Vector3.Distance(startMousePosition, Input.mousePosition) > 5)
                {
                    SelectUnitsInDragArea();
                }
                else
                {
                    SelectSingleUnitUnderMouse();
                }

                if (selectionBox) selectionBox.gameObject.SetActive(false);
            }

            if (isSelecting && selectionBox)
            {
                UpdateSelectionBox();
            }
        }

        private void DeselectAllUnits()
        {
            SelectDictionary.Instance.DeselectAll();
        }

        private void SelectUnitsInDragArea()
        {
            Vector3 lowerLeft = _camera.ScreenToWorldPoint(new Vector3(
                Mathf.Min(startMousePosition.x, Input.mousePosition.x),
                Mathf.Min(startMousePosition.y, Input.mousePosition.y), _camera.nearClipPlane));
            Vector3 upperRight = _camera.ScreenToWorldPoint(new Vector3(
                Mathf.Max(startMousePosition.x, Input.mousePosition.x),
                Mathf.Max(startMousePosition.y, Input.mousePosition.y), _camera.nearClipPlane));

            foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (unit.transform.position.x > lowerLeft.x && unit.transform.position.x < upperRight.x &&
                    unit.transform.position.y > lowerLeft.y && unit.transform.position.y < upperRight.y)
                {
                    SelectUnit(unit);
                }
            }
        }

        private void SelectSingleUnitUnderMouse()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayerMask))
            {
                SelectUnit(hit.collider.gameObject);
            }
        }

        private void SelectUnit(GameObject unit)
        {
            if (!SelectDictionary.Instance.selectUnits.ContainsValue(unit))
            {
                SelectDictionary.Instance.AddSelected(unit);
            }
        }

        private void UpdateSelectionBox()
        {
            float width = Input.mousePosition.x - startMousePosition.x;
            float height = Input.mousePosition.y - startMousePosition.y;

            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = startMousePosition + new Vector3(width / 2, height / 2);
        }
    }
}