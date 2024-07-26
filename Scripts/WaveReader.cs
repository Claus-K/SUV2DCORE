using System.IO;
using UnityEngine;

public abstract class WaveReader
{
    public static WaveData LoadWaveData(string fileName, string path)
    {
        // Sample Path :: "Scene/Campaing/CS1"
        var filePath = Path.Combine(Application.dataPath, path, fileName);
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<WaveData>(json);
        }

        Debug.LogError($"Couldn't find WAVE FILE --> '{fileName}'\n PATH :: '{path}'");
        return null;
    }
}