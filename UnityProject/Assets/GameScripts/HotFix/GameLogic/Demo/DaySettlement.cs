using GameLogic;
using TEngine;

/// <summary>
/// 统一处理进入新一天时需要执行的游戏数据结算。
/// </summary>
public static class DaySettlement
{
    private const int DailySatietyConsumption = 3;

    public static void Settle()
    {
        GameDataManager.Instance.Day++;

        for (int i = 0; i < GameDataManager.Instance.CharacterDataList.Count; i++)
        {
            CharacterData characterData = GameDataManager.Instance.CharacterDataList[i];
            if (characterData != null)
            {
                characterData.Satiety -= DailySatietyConsumption;

                if (characterData.Satiety <= 0)
                {
                    characterData.Hp -= 3;
                }
            }
        }
        
        GameEvent.EventMgr.GetInterface<IGameDataEvent>().DataChanged();
    }
}