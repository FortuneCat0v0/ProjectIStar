using GameConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public partial class DialogueUI
    {
        protected override void OnCreate()
        {
            m_goChoice.transform.SetParent(gameObject.transform);
            m_goChoice.SetActive(false);
            m_tfChoiceList.gameObject.SetActive(false);
        }

        private partial void OnClickContinueBtn()
        {
            DialogueManager.Instance.Continue();
        }

        public void ShowDialogue()
        {
            string id = DialogueManager.Instance.CurrentDialogueId;
            DialogueRow dialogueRow = ConfigSystem.Instance.Tables.TbDialogue.GetOrDefault(id);
            if (dialogueRow == null)
            {
                return;
            }

            m_tfChoiceList.gameObject.SetActive(false);

            if (dialogueRow.Type == DialogueType.Line)
            {
                m_tmpContent.text = $"{dialogueRow.Speaker}：{dialogueRow.Text}";
            }

            if (dialogueRow.Type == DialogueType.Choice)
            {
                foreach (DialogueChoice dialogueRowChoice in dialogueRow.Choices)
                {
                    GameObject go = UnityEngine.Object.Instantiate(m_goChoice, m_tfChoiceList);
                    go.GetComponent<Button>().onClick.AddListener(() => { OnChoiceBtn(dialogueRowChoice.NextId); });
                    go.GetComponentInChildren<TextMeshProUGUI>().text = dialogueRowChoice.Text;
                    go.SetActive(true);
                }

                m_tfChoiceList.gameObject.SetActive(true);
            }
        }

        private void OnChoiceBtn(string nextId)
        {
            DialogueManager.Instance.SelectChoice(nextId);
        }
    }
}