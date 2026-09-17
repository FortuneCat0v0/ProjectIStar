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

        private partial void OnClickTestBtn()
        {
            DialogueManager.Instance.EnterDialogue("10002");
        }

        private void RefreshData()
        {
            GameDataManager dataManager = GameDataManager.Instance;
            m_tmpDay.text = $"天数：{dataManager.Day}";
            m_tmpTime.text = $"时间：{dataManager.Time / 60}:{dataManager.Time % 60:00}";
            m_tmpStamina.text = $"体力：{dataManager.Stamina}/{ConfigSystem.Instance.Tables.TbGlobal.MaxStamina}";
            m_tmpStress.text = $"压力：{dataManager.Stress}/{ConfigSystem.Instance.Tables.TbGlobal.MaxStress}";
            m_tmpMoney.text = $"金钱：{dataManager.Money}";

            RefreshCharacterData(dataManager.SisterData1, m_tmpSister1Name, m_tmpSister1Affection, m_tmpSister1Trust);
            RefreshCharacterData(dataManager.SisterData2, m_tmpSister2Name, m_tmpSister2Affection, m_tmpSister2Trust);
        }

        private static void RefreshCharacterData(CharacterData characterData, TextMeshProUGUI nameText, TextMeshProUGUI affectionText, TextMeshProUGUI trustText)
        {
            nameText.text = characterData.Name;
            affectionText.text = $"好感度:{characterData.Affection}";
            trustText.text = $"信任度:{characterData.Trust}";
        }
    }
}
