using System.Linq;
using UnityEngine;
using Cells;

namespace UnitSelection
{
    public class MoveSelectedUnits : MonoBehaviour
    {
        string[] enemiesTags = { "Virus", "Bacteria", "Fungus" };
        string[] cellsTags = { "CellDef", "BloodCell" };
        private UnityEngine.Camera _camera;

        [SerializeField] private GameObject pingPrefab;
        // private SelectUnit _selectUnitScript;

        private enum UnitMoveType
        {
            Move,
            Attack,
            Targeted,
            TargetedInfected
        }

        private bool isMitosing;

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
            // _selectUnitScript = GetComponent<SelectUnit>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftAlt))
            {
                MoveUnits(UnitMoveType.Move);
            }

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(1))
            {
                MoveUnits(UnitMoveType.Attack);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Mitosi();
            }
        }

        private void MoveUnits(UnitMoveType type)
        {
            Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Agents"));
            var isTarget = false;
            if (hit.collider)
            {
                if (enemiesTags.Contains(hit.transform.tag))
                {
                    type = UnitMoveType.Targeted;
                    isTarget = true;
                }
                else if (cellsTags.Contains(hit.transform.tag))
                {
                    var infectionComponent = hit.transform.GetComponent<InfectionComponentV2>();
                    if (infectionComponent && infectionComponent.Infected)
                    {
                        type = UnitMoveType.TargetedInfected;
                        isTarget = true;
                    }
                }
            }

            hit = isTarget ? hit : Physics2D.Raycast(mousePos, Vector2.zero);
            Vector3 targetPosition = hit.point;
            var selectedUnit = SelectDictionary.Instance.GetDictionary().Values;
            foreach (var selectionComponent in selectedUnit.Select(unit => unit.GetComponent<SelectionComponent>()))
            {
                switch (type)
                {
                    case UnitMoveType.Move:
                        CreatePing(targetPosition, Color.white);
                        selectionComponent.AssignedMove = true;
                        break;
                    case UnitMoveType.Attack:
                        CreatePing(targetPosition, Color.red);
                        selectionComponent.AssignedMoveAttack = true;
                        break;
                    case UnitMoveType.Targeted:
                        CreatePing(targetPosition, Color.red);
                        selectionComponent.AssignedAttack = true;
                        selectionComponent.assignedTarget = hit.transform;
                        break;
                    case UnitMoveType.TargetedInfected:
                        CreatePing(targetPosition, Color.green);
                        var celltypeComponent = selectionComponent.GetComponent<CellTypeIdentifier>();
                        if (celltypeComponent && celltypeComponent.Type == enumCellType.TCell)
                        {
                            selectionComponent.AssignedAttack = true;
                            selectionComponent.assignedTarget = hit.transform;
                        }
                        break;
                    default:
                        Debug.LogError(
                            "Invalid Movement of Units, not defined inside ENUM or case. Script :: <MoveSelectedUnits.cs>");
                        break;
                }

                selectionComponent.destination = targetPosition;
            }
        }

        private void Mitosi()
        {
            int amount = SelectDictionary.Instance.selectUnits.Count;
            switch (amount)
            {
                case 1:
                {
                    var selectedUnit = SelectDictionary.Instance.selectUnits.Values;
                    foreach (var mitosi in selectedUnit.Select(unit => unit.GetComponent<MitosiComponent>())
                                 .Where(mitosi => mitosi))
                    {
                        mitosi.Mitosi();
                    }

                    break;
                }
                case > 1:
                    // NOTIFICATION LOG
                    Notification.Instance.SetNotification("Select a single Cells.");
                    break;
            }
        }

        private void CreatePing(Vector3 positon, Color color)
        {
            GameObject ping = Instantiate(pingPrefab, positon, Quaternion.identity);
            var spriteRenderer = ping.GetComponent<SpriteRenderer>();
            float origianlAlpha = spriteRenderer.color.a;
            color.a = origianlAlpha;
            spriteRenderer.color = color;
        }
    }
}