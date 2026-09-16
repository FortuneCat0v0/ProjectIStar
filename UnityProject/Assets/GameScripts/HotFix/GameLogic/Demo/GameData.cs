using System.Collections.Generic;
using GameLogic;

public class CourseData
{
    public int CourseId;
    public int Program;
}

public class CharacterData
{
    // 好感度
    public int Affection;
    // 信任度
    public int Trust;
    // 体魄
    public int Physique;
    // 智力
    public int Intelligence;
    // 情感
    public int Sensitivity;
    // 想象力
    public int Imagination;
    
    // 可以学习的课程
    public List<CourseData> AvailableCourses = new();
    // 已学完的课程
    public List<CourseData> CompletedCourses = new();
}

/// <summary>
/// 记录当前游戏进程中的公共数据。
/// </summary>
public sealed class GameData : Singleton<GameData>
{
    // 体力
    public int Stamina { get; set; }
    // 压力
    public int Stress { get; set; }
    // 金钱
    public int Money { get; set; }
    // 当前天数
    public int Day { get; set; }
    
    public CharacterData SisterData1 { get; set; }
    public CharacterData SisterData2 { get; set; }

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
        Money = 0;
        Day = 0;
        SisterData1 = new CharacterData();
        SisterData2 = new CharacterData();
    }
}