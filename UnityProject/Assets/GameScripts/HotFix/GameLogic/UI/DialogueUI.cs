using GameConfig;
using TEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public partial class DialogueUI
    {
        public string NodeId;

        protected override void OnCreate()
        {
            m_goChoice.transform.SetParent(gameObject.transform);
            m_goChoice.SetActive(false);
            m_tfChoiceList.gameObject.SetActive(false);
        }

        private partial void OnClickContinueBtn()
        {
            GameEvent.Send(IGame_Event.DialogueFinished, NodeId);
        }

        public void ShowDialogue(string id)
        {
            NodeId = id;

            GameplayNodeRow gameplayNodeRow = ConfigSystem.Instance.Tables.TbGameplayNode.GetOrDefault(id);

            m_tfChoiceList.gameObject.SetActive(false);

            m_tmpContent.text = $"{gameplayNodeRow.DialogueSpeaker}：{gameplayNodeRow.DialogueText}";
        }

        public void ShowChoice(string id)
        {
            NodeId = id;

            GameplayNodeRow gameplayNodeRow = ConfigSystem.Instance.Tables.TbGameplayNode.GetOrDefault(id);

            m_tfChoiceList.gameObject.SetActive(false);

            foreach (DialogueChoice dialogueRowChoice in gameplayNodeRow.DialogueChoices)
            {
                GameObject go = UnityEngine.Object.Instantiate(m_goChoice, m_tfChoiceList);
                go.GetComponent<Button>().onClick.AddListener(() => { OnChoiceBtn(dialogueRowChoice.Index); });
                go.GetComponentInChildren<TextMeshProUGUI>().text = dialogueRowChoice.Text;
                go.SetActive(true);
            }

            m_tfChoiceList.gameObject.SetActive(true);
        }


        private void OnChoiceBtn(int index)
        {
            GameEvent.Send(IGame_Event.DialogueChoice, NodeId, index);
        }
    }
}