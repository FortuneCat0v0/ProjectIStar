using System.Collections.Generic;
using GameConfig;
using TMPro;
using TEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public sealed class ItemInteractionData
    {
        public ItemInteractionData(ItemData itemData, Vector3 worldPosition)
        {
            ItemData = itemData;
            WorldPosition = worldPosition;
        }

        public ItemData ItemData { get; }
        public Vector3 WorldPosition { get; }
    }

    [Window(UILayer.Top)]
    public partial class InteractionUI
    {
        private readonly List<GameObject> _interactionItems = new();
        private RectTransform _interactionList;
        private ItemData _selectedItem;

        protected override void OnCreate()
        {
            _interactionList = m_tfInteractionList as RectTransform;
            m_itemInteract.SetActive(false);
        }

        protected override void OnRefresh()
        {
            if (!(UserData is ItemInteractionData interactionData) ||
                interactionData.ItemData == null || _interactionList == null ||
                !GameDataManager.Instance.ItemDataList.Contains(interactionData.ItemData))
            {
                Close();
                return;
            }

            ItemRow itemRow = ConfigSystem.Instance.Tables.TbItem.GetOrDefault(interactionData.ItemData.itemId);
            if (itemRow == null)
            {
                Log.Error($"InteractionUI cannot find item config {interactionData.ItemData.itemId}.");
                Close();
                return;
            }

            _selectedItem = interactionData.ItemData;
            IReadOnlyList<CharacterData> characters = GameDataManager.Instance.CharacterDataList;
            EnsureInteractionItemCount(characters.Count);
            for (int i = 0; i < characters.Count; i++)
            {
                CharacterData character = characters[i];
                GameObject interactionItem = _interactionItems[i];
                Button button = interactionItem.GetComponentInChildren<Button>(true);
                TextMeshProUGUI label = interactionItem.GetComponentInChildren<TextMeshProUGUI>(true);
                if (button == null || label == null)
                {
                    Log.Error("InteractionUI item requires a Button and TextMeshProUGUI.");
                    Close();
                    return;
                }

                label.text = $"给{character.Name}吃 +{itemRow.Satiety}";
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => UseSelectedItem(character, itemRow.Satiety));
                interactionItem.SetActive(true);
            }

            if (characters.Count == 0)
            {
                Close();
                return;
            }

            ResizeInteractionList(characters.Count);
            PositionInteractionList(interactionData.WorldPosition);
        }

        private partial void OnClickCloseBtn()
        {
            Close();
        }
        
        private void EnsureInteractionItemCount(int count)
        {
            while (_interactionItems.Count < count)
            {
                GameObject item = Object.Instantiate(m_itemInteract, m_tfInteractionList);
                item.name = $"InteractionItem_{_interactionItems.Count + 1}";
                _interactionItems.Add(item);
            }

            for (int i = count; i < _interactionItems.Count; i++)
            {
                _interactionItems[i].SetActive(false);
            }
        }

        private void ResizeInteractionList(int count)
        {
            RectTransform templateRect = m_itemInteract.transform as RectTransform;
            VerticalLayoutGroup layout = _interactionList.GetComponent<VerticalLayoutGroup>();
            if (templateRect == null)
            {
                return;
            }

            float spacing = layout != null ? layout.spacing : 0f;
            float padding = layout != null ? layout.padding.vertical : 0f;
            float height = count * templateRect.rect.height + Mathf.Max(0, count - 1) * spacing + padding;
            _interactionList.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, templateRect.rect.width);
            _interactionList.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private void PositionInteractionList(Vector3 worldPosition)
        {
            Camera worldCamera = Camera.main;
            RectTransform parentRect = _interactionList.parent as RectTransform;
            if (worldCamera == null || parentRect == null)
            {
                return;
            }

            Vector2 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);
            Canvas canvas = _interactionList.GetComponentInParent<Canvas>();
            Camera uiCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay
                ? canvas.worldCamera
                : null;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect, screenPosition, uiCamera, out Vector2 localPosition))
            {
                _interactionList.localPosition = localPosition;
            }
        }

        private void UseSelectedItem(CharacterData characterData, int satiety)
        {
            ItemData itemData = _selectedItem;
            _selectedItem = null;
            if (itemData != null)
            {
                GameDataManager.Instance.TryUseItem(itemData, characterData, satiety);
            }

            Close();
        }
    }
}
