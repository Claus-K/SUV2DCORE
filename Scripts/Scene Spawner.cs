using System;
using System.Collections;
using System.Collections.Generic;
using Cells;
using Unity.Mathematics;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    public static SceneSpawner Instance { get; private set; }

    [SerializeField] private GameObject leftSide;
    private Vector3[] _listLeftSide;

    [SerializeField] private GameObject rightSide;
    private Vector3[] _listRightSide;

    [SerializeField] private GameObject upSide;
    private Vector3[] _listUpSide;

    [SerializeField] private GameObject downSide;
    private Vector3[] _listDownSide;

    [SerializeField] private GameObject Target;

    private List<GameObject> SpawnedUnits;

    public static event Action<bool> GameOverWon;
    private bool isWaveChecking;
    private bool isFinalWave;

    public enum SpawnerSide
    {
        Left,
        Right,
        Up,
        Down
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _listLeftSide = InitLists(leftSide);
        _listRightSide = InitLists(rightSide);
        _listUpSide = InitLists(upSide);
        _listDownSide = InitLists(downSide);
        SpawnedUnits = new List<GameObject>();
        Target.GetComponent<SpriteRenderer>().enabled = false;
    }

    private Vector3[] InitLists(GameObject parent)
    {
        var list = new Vector3[parent.transform.childCount];
        for (var i = 0; i < parent.transform.childCount; i++)
        {
            list[i] = parent.transform.GetChild(i).position;
            parent.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }

        return list;
    }

    public IEnumerator Spawn(GameObject prefab, int amount, SpawnerSide side, bool isFinal = false)
    {
        var iter = 0;
        Vector3[] spawnList = null;
        switch (side)
        {
            case SpawnerSide.Left:
                spawnList = _listLeftSide;
                break;
            case SpawnerSide.Right:
                spawnList = _listRightSide;

                break;
            case SpawnerSide.Up:
                spawnList = _listUpSide;
                break;
            case SpawnerSide.Down:
                spawnList = _listDownSide;
                break;
            default:
                Debug.Log("Spawner Script Side to Spawn Not setup");
                break;
        }

        if (spawnList == null)
            yield break;

        while (amount > 0)
        {
            spawnPrefab(prefab, spawnList, iter);
            amount -= 1;
            iter += 1;
            if (iter >= spawnList.Length)
            {
                iter = 0;
            }

            yield return new WaitForSeconds(0.5f);
        }

        if (isFinal)
        {
            isFinalWave = true;
        }

        if (!isWaveChecking)
        {
            StartCoroutine(WaveCheck());
        }
    }

    private void spawnPrefab(GameObject prefab, Vector3[] list, int iter)
    {
        var it = Instantiate(prefab, list[iter], Quaternion.identity);
        var sc = it.GetComponent<SelectionComponent>();
        SpawnedUnits.Add(it);
        sc.AssignedMoveAttack = true;
        sc.destination = Target.transform.position;
    }

    private IEnumerator WaveCheck()
    {
        isWaveChecking = true;
        while (SpawnedUnits.Count > 0)
        {
            SpawnedUnits.RemoveAll(item => item == null);
            // Debug.Log($"UNITS REMAINING --> {SpawnedUnits.Count}");
            yield return new WaitForSeconds(2f);
        }

        // Debug.Log($"FINAL :: {isFinalWave}");
        if (isFinalWave)
        {
            GameOverWon?.Invoke(false);
        }

        isWaveChecking = false;
    }

    private void OnDestroy()
    {
        SpawnedUnits.Clear();
    }
}