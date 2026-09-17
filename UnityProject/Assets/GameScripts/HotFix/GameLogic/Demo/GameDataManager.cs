using System.Collections.Generic;
using GameLogic;
using TEngine;

public class CourseData
{
    public int CourseId;
    public int Program;
}

public class CharacterData
{
    public string Name;

    private int _affection;
    private int _trust;
    
    // 好感度
    public int Affection
    {
        get => _affection;
        set
        {
            _affection = value;
            GameEvent.EventMgr.GetInterface<IGameDataEvent>().DataChanged();
        }
    }
    // 信任度
    public int Trust
    {
        get => _trust;
        set
        {
            _trust = value;
            GameEvent.EventMgr.GetInterface<IGameDataEvent>().DataChanged();
        }
    }

    // -----先不用-----
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
public sealed class GameDataManager : Singleton<GameDataManager>
{
    private int _day;
    private int _time;
    private int _stamina;
    private int _stress;
    private int _money;

    // 当前天数
    public int Day { get => _day; set => SetValue(ref _day, value); }
    // 时间，单位为分钟
    public int Time { get => _time; set => SetValue(ref _time, value); }
    // 体力
    public int Stamina { get => _stamina; set => SetValue(ref _stamina, value); }
    // 压力
    public int Stress { get => _stress; set => SetValue(ref _stress, value); }
    // 金钱
    public int Money { get => _money; set => SetValue(ref _money, value); }
    
    public CharacterData SisterData1 { get; set; }
    public CharacterData SisterData2 { get; set; }

    protected override void OnInit()
    {
        Reset();
        GameEvent.EventMgr.GetInterface<IGameDataEvent>().DataChanged();
    }

    protected override void OnRelease()
    {
    }

    private void SetValue(ref int field, int value)
    {
        if (field == value)
        {
            return;
        }

        field = value;
        GameEvent.EventMgr.GetInterface<IGameDataEvent>().DataChanged();
    }

    /// <summary>
    /// 将所有游戏数据恢复为初始值。
    /// </summary>
    public void Reset()
    {
        _stamina = ConfigSystem.Instance.Tables.TbGlobal.MaxStress;
        _stress = 0;
        _money = 100;
        _day = 1;
        _time = 7 * 60;
        SisterData1 = new CharacterData();
        SisterData1.Name = "NPC1";
        SisterData2 = new CharacterData();
        SisterData2.Name = "NPC2";
    }
}
