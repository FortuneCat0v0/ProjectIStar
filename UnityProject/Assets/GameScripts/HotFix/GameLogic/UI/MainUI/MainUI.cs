using UnityEngine;
using UnityEngine.UI;
using TEngine;

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
        }
    }
}
