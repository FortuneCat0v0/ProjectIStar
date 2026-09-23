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
    public int CharacterId { get; private set; }
    public string Name => ConfigSystem.Instance.Tables.TbCharacter.Get(CharacterId).Name;

    private int _affection;
    private int _trust;
    private int _hp;
    private int _satiety;

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

    // 生命
    public int Hp
    {
        get => _hp;
        set => _hp = value > 0 ? value : 0;
    }

    // 饱腹
    public int Satiety
    {
        get => _satiety;
        set => _satiety = value > 0 ? value : 0;
    }

    // 压力
    public int Stress;

    public CharacterData(int id)
    {
        CharacterId = id;
        Hp = 10;
        Satiety = 7;
    }
}

public class ItemData
{
    public int itemId;
}

/// <summary>
/// 记录当前游戏进程中的公共数据。
/// </summary>
public sealed class GameDataManager : Singleton<GameDataManager>
{
    private int _day;
    private int _time;
    private int _stamina;

    // 当前天数
    public int Day
    {
        get => _day;
        set => SetValue(ref _day, value);
    }

    // 时间，单位为分钟
    public int Time
    {
        get => _time;
        set => SetValue(ref _time, value);
    }

    // 体力
    public int Stamina
    {
        get => _stamina;
        set => SetValue(ref _stamina, value);
    }

    public List<CharacterData> CharacterDataList = new();
    public List<ItemData> ItemDataList = new();

    /// <summary>
    /// 消耗指定道具，并把饱腹值应用到当前角色列表中的目标角色。
    /// </summary>
    public bool TryUseItem(ItemData itemData, CharacterData characterData, int satiety)
    {
        if (itemData == null || characterData == null ||
            !CharacterDataList.Contains(characterData) || !ItemDataList.Remove(itemData))
        {
            return false;
        }

        characterData.Satiety += satiety;
        GameEvent.EventMgr.GetInterface<IGameDataEvent>().DataChanged();
        return true;
    }

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
        _day = 1;
        _time = 7 * 60;
        _stamina = 7;
        CharacterData characterData1 = new CharacterData(ConfigSystem.Instance.Tables.TbGlobal.CharacterIdMy);
        CharacterData characterData2 = new CharacterData(ConfigSystem.Instance.Tables.TbGlobal.CharacterIdErGou);
        CharacterData characterData3 = new CharacterData(ConfigSystem.Instance.Tables.TbGlobal.CharacterIdCuiHua);
        CharacterDataList.Clear();
        CharacterDataList.Add(characterData1);
        CharacterDataList.Add(characterData2);
        CharacterDataList.Add(characterData3);

        ItemDataList.Clear();
        for (int i = 0; i < 5; i++)
        {
            ItemData itemData = new ItemData();
            itemData.itemId = 10001;

            ItemDataList.Add(itemData);
        }
    }
}