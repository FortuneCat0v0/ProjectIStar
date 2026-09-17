using Cysharp.Threading.Tasks;
using GameConfig;
using TEngine;

namespace GameLogic
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        public string CurrentDialogueId { get; private set; }

        protected override void OnInit()
        {
        }

        public void EnterDialogue(string id)
        {
            DialogueRow dialogueRow = ConfigSystem.Instance.Tables.TbDialogue.GetOrDefault(id);
            if (dialogueRow == null)
            {
                Log.Error("Dialogue ID " + id + " not found");
                return;
            }

            CurrentDialogueId = id;

            if (dialogueRow.Type == DialogueType.Line)
            {
                ShowLine().Forget();
            }

            if (dialogueRow.Type == DialogueType.Choice)
            {
                ShowLine().Forget();
            }

            if (dialogueRow.Type == DialogueType.Condition)
            {
            }

            if (dialogueRow.Type == DialogueType.Command)
            {
            }

            if (dialogueRow.Type == DialogueType.Jump)
            {
            }

            if (dialogueRow.Type == DialogueType.End)
            {
                CurrentDialogueId = null;
                GameModule.UI.CloseUI<DialogueUI>();
            }
        }

        private async UniTask ShowLine()
        {
            GameModule.UI.ShowUI<DialogueUI>();
            DialogueUI ui = await GameModule.UI.GetUIAsyncAwait<DialogueUI>();
            ui.ShowDialogue();
        }

        public void Continue()
        {
            if (CurrentDialogueId == null)
            {
                return;
            }

            DialogueRow dialogueRow = ConfigSystem.Instance.Tables.TbDialogue.GetOrDefault(CurrentDialogueId);

            if (dialogueRow.Type == DialogueType.Line)
            {
                EnterDialogue(dialogueRow.NextId);
            }
        }

        public void SelectChoice(string id)
        {
            EnterDialogue(id);
        }
    }
}