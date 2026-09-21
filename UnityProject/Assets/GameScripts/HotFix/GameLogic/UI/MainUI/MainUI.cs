using System.Collections.Generic;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public partial class MainUI
    {
        private readonly List<MainCharacterDataWidget> _characterDataWidgets = new();

        protected override void RegisterEvent()
        {
            AddUIEvent(IGameDataEvent_Event.DataChanged, RefreshData);
        }

        protected override void OnCreate()
        {
            m_itemCharacterData.SetActive(false);
        }

        protected override void OnRefresh()
        {
            RefreshData();
        }

        private partial void OnClickMapBtn()
        {
            GameModule.UI.ShowUI<MapUI>();
        }

        private partial void OnClickTestBtn()
        {
            GameplayManager.Instance.EnterGraph(10001);
        }

        private void RefreshData()
        {
            GameDataManager dataManager = GameDataManager.Instance;
            m_tmpDay.text = $"天数：{dataManager.Day}";
            m_tmpTime.text = $"时间：{dataManager.Time / 60}:{dataManager.Time % 60:00}";
            m_tmpStamina.text = $"体力：{dataManager.Stamina}/{ConfigSystem.Instance.Tables.TbGlobal.MaxStamina}";
            m_tmpStress.text = $"压力：{dataManager.Stress}/{ConfigSystem.Instance.Tables.TbGlobal.MaxStress}";
            m_tmpMoney.text = $"金钱：{dataManager.Money}";

            RefreshCharacterDataList(dataManager.CharacterDataList);
        }

        private void RefreshCharacterDataList(IReadOnlyList<CharacterData> characterDataList)
        {
            int characterCount = characterDataList?.Count ?? 0;
            AdjustIconNum(_characterDataWidgets, characterCount, m_tfCharacterData, m_itemCharacterData);

            for (int i = 0; i < characterCount; i++)
            {
                _characterDataWidgets[i].SetData(characterDataList[i]);
            }
        }
    }
}