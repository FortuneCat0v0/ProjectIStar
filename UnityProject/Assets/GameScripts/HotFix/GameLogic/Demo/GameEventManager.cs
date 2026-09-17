using System.Collections.Generic;
using GameConfig;
using TEngine;

namespace GameLogic
{
    public class GameEventManager : Singleton<GameDataManager>
    {
        public int CurrentEventId { get; set; }

        public List<int> PendingEvents = new();

        protected override void OnInit()
        {
        }

        public void TriggerEvent(int eventId)
        {
            // 判断当前条件是否能执行该事件。。。

            CurrentEventId = eventId;

            EventRow eventRow = ConfigSystem.Instance.Tables.TbEvent.GetOrDefault(eventId);
            if (eventRow == null)
            {
                return;
            }
        }
    }
}