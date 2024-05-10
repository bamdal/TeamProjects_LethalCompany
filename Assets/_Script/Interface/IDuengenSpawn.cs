
public interface  IDuengenSpawn
{
    /// <summary>
    /// 최대 스폰 가능한 마릿수
    /// </summary>
    public int MaxSpawnCount { get; set; }

    /// <summary>
    /// 게임내에 1개의 개체가 스폰될 확률
    /// </summary>
    public float SpawnPercent { get; set; }



}
