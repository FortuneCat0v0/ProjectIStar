using System.Collections.Generic;
using GameConfig;
using TEngine;

namespace GameLogic
{
    public class GameEventManager : Singleton<GameDataManager>
    {
        public int CurrentEventId { get; set; }
        public int CurrentEffectId { get; set; }

        public List<int> PendingEvents = new();

        protected override void OnInit()
        {
        }
        
        public void TriggerEvent(int eventId)
        {
            if (CurrentEffectId != 0)
            {
                Log.Warning("有事件正在执行");
                return;
            }
            
            // 判断当前条件是否能执行该事件。。。
            
            CurrentEventId = eventId;

            EventRow eventRow = ConfigSystem.Instance.Tables.TbEvent.GetOrDefault(eventId);
            if (eventRow == null)
            {
                return;
            }
            
            CurrentEffectId = eventRow.EffectId;
            OnActionEffect();
        }

        private void OnActionEffect()
        {
            EffectRow effectRow = ConfigSystem.Instance.Tables.TbEffect.GetOrDefault(CurrentEffectId);
            if (effectRow.Type == EffectActionType.None)
            {
                OnFinishEvent();
                return;
            }

            if (effectRow.Type == EffectActionType.Dialogue)
            {
                // TODO 显示对话
            }
        }

        private void OnFinishEvent()
        {
            CurrentEventId = 0;
            CurrentEffectId = 0;
        }
    }
}