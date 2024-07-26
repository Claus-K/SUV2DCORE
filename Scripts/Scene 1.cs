using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scene1 : MonoBehaviour
{
    [SerializeField] private Enemy.EnemyVirusDatabase virus;
    private WaveData _waveData;
    [SerializeField] private string fileName;
    [SerializeField] private string path;

    private void Start()
    {
        _waveData = WaveReader.LoadWaveData(fileName, path);
        if (_waveData != null)
        {
            StartCoroutine(Wave());
        }
    }

    private IEnumerator Wave()
    {
        for (var i = 0; i < _waveData.waves.Count; i++)
        {
            yield return new WaitForSeconds(_waveData.waves[i].delay);
            foreach (var enemy in _waveData.waves[i].enemies)
            {
                var prefab =
                    virus.GetPrefab((Enemy.enumVirusType)System.Enum.Parse(typeof(Enemy.enumVirusType), enemy.type));
                var side = (SceneSpawner.SpawnerSide)System.Enum.Parse(typeof(SceneSpawner.SpawnerSide), enemy.side);
                StartCoroutine(i == _waveData.waves.Count - 1
                    ? SceneSpawner.Instance.Spawn(prefab, enemy.count, side, true)
                    : SceneSpawner.Instance.Spawn(prefab, enemy.count, side));
                // var prefab = virus.GetPrefab(Enemy.enumVirusType.A);
                // StartCoroutine(SceneSpawner.Instance.Spawn(prefab, 10, SceneSpawner.SpawnerSide.Right));
                // yield return new WaitForSeconds(5);
            }
        }
    }
}