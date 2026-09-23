using UnityEngine;

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
            if (_itemData == null || GameModule.UI.IsPointerOverUI())
            {
                return;
            }

            GameModule.UI.ShowUI<InteractionUI>(new ItemInteractionData(_itemData, transform.position));
        }
    }
}
