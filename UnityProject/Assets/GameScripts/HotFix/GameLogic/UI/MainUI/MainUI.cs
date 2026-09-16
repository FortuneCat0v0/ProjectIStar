using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public partial class MainUI
    {
        protected override void RegisterEvent()
        {
            AddUIEvent(IGameDataEvent_Event.DataChanged, RefreshData);
        }

        protected override void OnRefresh()
        {
            RefreshData();
        }

        private void RefreshData()
        {
            GameData data = GameData.Instance;
            m_tmpDay.text = $"天数：{data.Day}";
            m_tmpTime.text = $"时间：{data.Time / 60}:{data.Time % 60:00}";
            m_tmpStamina.text = $"体力：{data.Stamina}/{ConfigSystem.Instance.Tables.TbGlobal.MaxStamina}";
            m_tmpStress.text = $"压力：{data.Stress}/{ConfigSystem.Instance.Tables.TbGlobal.MaxStress}";
            m_tmpMoney.text = $"金钱：{data.Money}";

            RefreshCharacterData(data.SisterData1, m_tmpSister1Name, m_tmpSister1Affection, m_tmpSister1Trust);
            RefreshCharacterData(data.SisterData2, m_tmpSister2Name, m_tmpSister2Affection, m_tmpSister2Trust);
        }

        private static void RefreshCharacterData(CharacterData characterData, TextMeshProUGUI nameText, TextMeshProUGUI affectionText, TextMeshProUGUI trustText)
        {
            nameText.text = characterData.Name;
            affectionText.text = $"好感度:{characterData.Affection}";
            trustText.text = $"信任度:{characterData.Trust}";
        }
    }
}
