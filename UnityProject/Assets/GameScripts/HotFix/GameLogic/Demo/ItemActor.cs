using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic
{
    public class ItemActor : MonoBehaviour
    {
        private ItemData _itemData;

        public void Initialize(ItemData itemData)
        {
            _itemData = itemData;
        }

        private void OnMouseDown()
        {
            if (_itemData == null || (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
            {
                return;
            }

            GameModule.UI.ShowUI<InteractionUI>(new ItemInteractionData(_itemData, transform.position));
        }
    }
}