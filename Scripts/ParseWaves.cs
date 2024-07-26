using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public string type;
    public int count;
    public string side;
}

[System.Serializable]
public class EnemyWave
{
    public float delay;
    public List<EnemyData> enemies;
}

[System.Serializable]
public class WaveData
{
    public List<EnemyWave> waves;
}