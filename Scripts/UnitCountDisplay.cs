using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cells;
using NavMeshPlus.Components;
using TMPro;
using UIElements;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountDisplay : MonoBehaviour
{
    // BUTTONS

    public Button _BcellButton;
    [SerializeField] private TextMeshProUGUI _Bcell;

    public Button _MacrophageButton;
    [SerializeField] private TextMeshProUGUI _Macrophage;

    public Button _TcellButton;
    [SerializeField] private TextMeshProUGUI _Tcell;

    public Button _DendriticButton;
    [SerializeField] private TextMeshProUGUI _Dendritic;

    public Button _MemoryButton;
    [SerializeField] private TextMeshProUGUI _Memory;
    
    // private  ButtonLayout _layout;

    // private void Awake()
    // {
        // _layout = _Bcell.GetComponentInParent<ButtonLayout>();
    // }

    private void OnEnable()
    {
        ScenePoints.OnUnitAdded += UpdateDisplay;
        ScenePoints.OnUnitRemoved += UpdateDisplay;
    }

    private void OnDisable()
    {
        ScenePoints.OnUnitAdded -= UpdateDisplay;
        ScenePoints.OnUnitRemoved -= UpdateDisplay;
    }

    private void UpdateDisplay(enumCellType type)
    {
        // Debug.Log($"Updating Display :: {type}");
        var count = ScenePoints.Instance.CellsList.TryGetValue(type, out var value) ? value.Count : 0;
        switch (type)
        {
            case enumCellType.BCell:
                SetButtonState(_BcellButton, _Bcell, count);
                break;
            case enumCellType.Macrophage:
                SetButtonState(_MacrophageButton, _Macrophage, count);
                break;
            case enumCellType.TCell:
                SetButtonState(_TcellButton, _Tcell, count);
                break;
            case enumCellType.Dendritic:
                SetButtonState(_DendriticButton, _Dendritic, count);
                break;
            case enumCellType.Memory:
                SetButtonState(_MemoryButton, _Memory, count);
                break;
            default:
                Debug.Log(
                    $"type of cell is not included in COUNT DISPLAY --> UPDATE UNIT DISPLAY to contain {type}");
                break;
        }

        // if (_layout)
        // {
        //     _layout.RearrangeButtons();
        // }
        
    }


    private void SetButtonState(Button button, TextMeshProUGUI textMesh, int count)
    {
        button.gameObject.SetActive(count != 0);
        textMesh.text = count.ToString();
        
    }
}