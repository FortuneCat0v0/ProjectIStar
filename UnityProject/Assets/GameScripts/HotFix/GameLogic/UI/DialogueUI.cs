using GameConfig;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public partial class DialogueUI
    {
        private partial void OnClickContinueBtn()
        {
            DialogueManager.Instance.Continue();
        }

        public void ShowDialogue()
        {
            int id = DialogueManager.Instance.CurrentDialogueId;
            DialogueRow dialogueRow = ConfigSystem.Instance.Tables.TbDialogue.GetOrDefault(id);
            if (dialogueRow == null)
            {
                return;
            }

            if (dialogueRow.Type == DialogueType.Line)
            {
                m_tmpContent.text = $"{dialogueRow.Speaker}：{dialogueRow.Text}";
            }
        }
    }
}