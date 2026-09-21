using TEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupLogic)]
    public interface IGame
    {
        void DialogueFinished(string id);
        void DialogueChoice(string id, int index);
    }
}