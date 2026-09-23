using System;
using Cysharp.Threading.Tasks;
using GameConfig;
using TEngine;

namespace GameLogic
{
    public class GameplayManager : SingletonBehaviour<GameplayManager>
    {
        private int CurrentGraphId;
        private string CurrentNodeId;
        private GameplayNodeBase CurrentNode;

        public bool IsDialogueActive => CurrentNode is DialogueNode || CurrentNode is ChoiceNode;

        private void Update()
        {
            if (CurrentNode != null)
            {
                CurrentNode.Update();
            }
        }

        public void TriggerDay(int day)
        {
            foreach (GameplayGraphRow gameplayGraphRow in ConfigSystem.Instance.Tables.TbGameplayGraph.DataList)
            {
                if (gameplayGraphRow.TriggerDay && gameplayGraphRow.TriggerDayParam == day)
                {
                    EnterGraph(gameplayGraphRow.Id);
                }
            }
        }
        
        public void EnterGraph(int id)
        {
            if (CurrentGraphId != 0)
            {
                Log.Error("当前有Graph正在运行");
                return;
            }

            GameplayGraphRow gameplayGraphRow = ConfigSystem.Instance.Tables.TbGameplayGraph.GetOrDefault(id);
            if (gameplayGraphRow == null)
            {
                Log.Error("Gameplay graph id " + id + " not found");
                return;
            }

            CurrentGraphId = id;

            string nodeId = gameplayGraphRow.RootNodeId;

            EnterNode(nodeId);
        }

        public void EnterNode(string nodeId)
        {
            GameplayNodeRow gameplayNodeRow = ConfigSystem.Instance.Tables.TbGameplayNode.GetOrDefault(nodeId);

            if (gameplayNodeRow == null)
            {
                Log.Error("Gameplay node id " + nodeId + " not found");
                return;
            }

            // 移除当前节点
            CurrentNode?.End();
            CurrentNode = null;

            GameplayNodeBase node = null;
            if (gameplayNodeRow.Type == GameplayNodeType.Root)
            {
                // 进入下一个节点
                node = new RootNode(nodeId);
            }

            if (gameplayNodeRow.Type == GameplayNodeType.Dialogue)
            {
                // 显示聊天文本
                node = new DialogueNode(nodeId);
            }

            if (gameplayNodeRow.Type == GameplayNodeType.Choice)
            {
                // 显示选择
                node = new ChoiceNode(nodeId);
            }

            if (gameplayNodeRow.Type == GameplayNodeType.End)
            {
                Log.Debug("Gameplay end " + CurrentGraphId);

                CurrentGraphId = 0;
                CurrentNodeId = null;

                GameModule.UI.CloseUI<DialogueUI>();

                return;
            }

            CurrentNode = node;
            CurrentNode?.Execute();
        }
    }

    public abstract class GameplayNodeBase
    {
        public string NodeId;
        public GameplayNodeRow GameplayNodeRow => ConfigSystem.Instance.Tables.TbGameplayNode.Get(NodeId);

        protected GameplayNodeBase(string nodeId)
        {
            NodeId = nodeId;
        }

        public virtual void Execute()
        {
            Log.Debug("Enter node " + NodeId);
        }

        public virtual void Update()
        {
        }

        public virtual void End()
        {
            Log.Debug("End node " + NodeId);
        }
    }

    public class RootNode : GameplayNodeBase
    {
        public RootNode(string nodeId) : base(nodeId)
        {
        }

        public override void Execute()
        {
            base.Execute();
            GameplayManager.Instance.EnterNode(GameplayNodeRow.NextId[0]);
        }

        public override void Update()
        {
        }

        public override void End()
        {
            base.End();
        }
    }

    public class DialogueNode : GameplayNodeBase
    {
        public DialogueNode(string nodeId) : base(nodeId)
        {
        }

        public override void Execute()
        {
            base.Execute();
            GameEvent.AddEventListener<string>(IGame_Event.DialogueFinished, OnDialogueFinished);

            ShowLine().Forget();
        }

        public override void Update()
        {
        }

        public override void End()
        {
            base.End();
            GameEvent.RemoveEventListener<string>(IGame_Event.DialogueFinished, OnDialogueFinished);
        }

        private async UniTask ShowLine()
        {
            GameModule.UI.ShowUI<DialogueUI>();
            DialogueUI ui = await GameModule.UI.GetUIAsyncAwait<DialogueUI>();
            ui.ShowDialogue(NodeId);
        }

        private void OnDialogueFinished(string id)
        {
            if (id != NodeId)
            {
                return;
            }

            GameplayManager.Instance.EnterNode(GameplayNodeRow.NextId[0]);
        }
    }

    public class ChoiceNode : GameplayNodeBase
    {
        public ChoiceNode(string nodeId) : base(nodeId)
        {
        }

        public override void Execute()
        {
            base.Execute();
            GameEvent.AddEventListener<string, int>(IGame_Event.DialogueChoice, OnDialogueChoice);

            ShowChoice().Forget();
        }

        public override void Update()
        {
        }

        public override void End()
        {
            base.End();
            GameEvent.RemoveEventListener<string, int>(IGame_Event.DialogueChoice, OnDialogueChoice);
        }

        private async UniTask ShowChoice()
        {
            GameModule.UI.ShowUI<DialogueUI>();
            DialogueUI ui = await GameModule.UI.GetUIAsyncAwait<DialogueUI>();
            ui.ShowChoice(NodeId);
        }

        private void OnDialogueChoice(string id, int index)
        {
            if (id != NodeId)
            {
                return;
            }

            GameplayManager.Instance.EnterNode(GameplayNodeRow.NextId[index]);
        }
    }
}
