using TEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupLogic)]
    public interface IGameDataEvent
    {
        void DataChanged();
    }
}
