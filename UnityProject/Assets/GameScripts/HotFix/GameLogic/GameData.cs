using GameLogic;

/// <summary>
/// 记录当前游戏进程中的公共数据。
/// </summary>
public sealed class GameData : Singleton<GameData>
{
    /// <summary>
    /// 当前体力。
    /// </summary>
    public int Stamina { get; set; }

    /// <summary>
    /// 当前压力值。
    /// </summary>
    public int Stress { get; set; }

    /// <summary>
    /// 当前金币数量。
    /// </summary>
    public int Gold { get; set; }

    /// <summary>
    /// 当前回合数。
    /// </summary>
    public int RoundCount { get; set; }

    protected override void OnInit()
    {
        Reset();
    }

    /// <summary>
    /// 将所有游戏数据恢复为初始值。
    /// </summary>
    public void Reset()
    {
        Stamina = 0;
        Stress = 0;
        Gold = 0;
        RoundCount = 0;
    }
}
