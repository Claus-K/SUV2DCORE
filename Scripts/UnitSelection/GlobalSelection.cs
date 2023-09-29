using System.Linq;
using UnityEngine;

namespace UnitSelection
{
    public class GlobalSelection : MonoBehaviour
    {
        private Vector3 startMousePosition;
        private UnityEngine.Camera _camera;
        private bool drawBox;
        private bool isShiftPressed;

        void Start()
        {
            _camera = UnityEngine.Camera.main;
            if (_camera == null)
            {
                Debug.LogError("Main Camera not Found!");
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isShiftPressed = true;
                if (Input.GetMouseButtonDown(0))
                {
                    startMousePosition = Input.mousePosition;
                }
                if (Input.GetMouseButton(0))
                {
                    drawBox = true;
                    SelectUnitBox();
                }
            }
            else if (isShiftPressed && !Input.GetKey(KeyCode.LeftShift))
            {
                drawBox = false;
                isShiftPressed = false;
            }

            if (Input.GetMouseButton(0) && !isShiftPressed)
            {
                SelectUnitUnder();
            }
        }

        private void SelectUnitUnder()
        {
            Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null &&
                hit.collider.gameObject.CompareTag("Player")) // Check if the raycast hit something
            {
                // Debug.Log("Raycast hit: " + hit.collider.gameObject.name); // Display name of hit object

                if (hit.collider.CompareTag("Player"))
                {
                    SelectDictionary.Instance.AddSelected(hit.collider.gameObject);
                }
            }
            else
            {
                SelectDictionary.Instance.DeselectAll();
                // Debug.Log("Raycast did not hit a valid object.");
            }
        }
        
        private void OnGUI()
        {
            if (!drawBox) return;
            // Debug.Log("GUI DRAWING");
            Rect screenRect = Utils.GetScreenRect(startMousePosition, Input.mousePosition);
            Utils.DrawScreenRect(screenRect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(screenRect, 2, Color.blue);
        }

        private void SelectUnitBox()
        {
            Vector2 boxStart = _camera.ScreenToWorldPoint(startMousePosition);
            Vector2 boxEnd = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 boxCenter = (boxStart + boxEnd) / 2;
            Vector2 boxSize = new Vector2(Mathf.Abs(boxEnd.x - boxStart.x), Mathf.Abs(boxEnd.y - boxStart.y));

            Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0);
            foreach (var hit in hits.Where(hit =>hit.CompareTag("Player")))
            {
                SelectDictionary.Instance.AddSelected(hit.gameObject);
            }
        }
    }
}