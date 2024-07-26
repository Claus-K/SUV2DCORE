using System;
using System.Collections;
using System.Collections.Generic;
using Cells;
using Enemy;
using UnitSelection;
using UnityEngine;

public class ScenePoints : MonoBehaviour
{
    public static ScenePoints Instance { get; private set; }
    public Dictionary<enumCellType, Dictionary<int, GameObject>> CellsList = new();
    public Dictionary<enumCellType, int> DeadCells = new();
    public Dictionary<enumVirusType, int> KilledVirus = new();
    public int SceneBloodCell { get; set; }
    public int BloodCellsCount { get; set; }
    private int points;
    // TODO Bacteria and Parasite

    public static event Action<enumCellType> OnUnitAdded;
    public static event Action<enumCellType> OnUnitRemoved;
    public static event Action Status;
    public static event Action<int, int> SetUnitInfor;
    public static event Action<int> UpdateUnitInfor;
    public static event Action BloodStatusCheck;

    public int Points
    {
        get => points;
        set => points = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddUnit(GameObject go, enumCellType type)
    {
        var id = go.GetInstanceID();
        // Debug.Log($"GETTING ID AT ADD UNIT :: {id}");
        if (!CellsList.ContainsKey(type))
        {
            CellsList[type] = new Dictionary<int, GameObject>();
        }

        // Debug.Log($"Global List {id} --> {type}");
        CellsList[type].Add(id, go);
        OnUnitAdded?.Invoke(type);
    }

    public void RemoveUnit(int id, enumCellType type)
    {
        if (!Instance) return;
        // if (CellsList[type].ContainsKey(id))
        // {
        CellsList[type].Remove(id);
        // }
        // else
        // {
        //     Debug.Log($"REMOVE UNIT FAILED :: {id}");
        // }

        DeadCount(type);
        OnUnitRemoved?.Invoke(type);
        Status?.Invoke();
    }

    private void DeadCount(enumCellType type)
    {
        if (!DeadCells.TryAdd(type, 1))
        {
            DeadCells[type] += 1;
        }
    }

    public void VirusKillCount(enumVirusType type)
    {
        if (!KilledVirus.TryAdd(type, 1))
        {
            KilledVirus[type] += 1;
        }
    }

    public void BloodIncDec(bool type)
    {
        switch (type)
        {
            case true:
                BloodCellsCount += 1;
                SceneBloodCell += 1;
                SetUnitInfor?.Invoke(BloodCellsCount, SceneBloodCell);
                break;
            case false:
                if (!Instance) break;
                BloodCellsCount -= 1;
                Notification.Instance.SetNotification("Blood Cell Has Died.");
                UpdateUnitInfor?.Invoke(BloodCellsCount);
                BloodStatusCheck?.Invoke();
                break;
        }
    }

    private void OnDestroy()
    {
        CellsList.Clear();
        DeadCells.Clear();
        KilledVirus.Clear();
    }
}