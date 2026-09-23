using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic
{
    /// <summary>
    /// Keeps the child models in sync with matching entries in the runtime item list.
    /// </summary>
    public sealed class ItemModelList : MonoBehaviour
    {
        [SerializeField] private int itemId = 10001;
        [SerializeField, Min(1)] private int columns = 5;
        [SerializeField] private Vector2 spacing = new(0.25f, 0.25f);

        private readonly List<ItemModelView> _views = new();
        private bool _loadFailed;

        private void Update()
        {
            if (!_loadFailed)
            {
                Synchronize(GameDataManager.Instance.ItemDataList);
            }
        }

        private void Synchronize(IReadOnlyList<ItemData> items)
        {
            bool changed = false;
            for (int i = _views.Count - 1; i >= 0; i--)
            {
                ItemModelView view = _views[i];
                if (view.ItemData == null || !ContainsReference(items, view.ItemData))
                {
                    RemoveViewAt(i);
                    changed = true;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                ItemData itemData = items[i];
                if (itemData == null || itemData.itemId != itemId || ContainsView(itemData))
                {
                    continue;
                }

                if (!TryCreateView(itemData))
                {
                    break;
                }

                changed = true;
            }

            if (changed)
            {
                ArrangeInstances();
            }
        }

        private bool TryCreateView(ItemData itemData)
        {
            ItemRow itemRow = ConfigSystem.Instance.Tables.TbItem.GetOrDefault(itemId);

            GameObject instance = GameModule.Resource.LoadGameObject(itemRow.Actor, transform);

            instance.name = $"{itemRow.Actor}_{_views.Count + 1}";
            ItemActor actor = instance.GetComponent<ItemActor>();
            actor.Initialize(itemData);
            _views.Add(new ItemModelView(itemData, instance));
            return true;
        }

        private bool ContainsView(ItemData itemData)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                if (ReferenceEquals(_views[i].ItemData, itemData))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsReference(IReadOnlyList<ItemData> items, ItemData itemData)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (ReferenceEquals(items[i], itemData))
                {
                    return true;
                }
            }

            return false;
        }

        private void RemoveViewAt(int index)
        {
            GameObject instance = _views[index].Instance;
            _views.RemoveAt(index);
            if (instance != null)
            {
                Destroy(instance);
            }
        }

        private void ArrangeInstances()
        {
            int columnCount = Mathf.Max(1, columns);
            for (int i = 0; i < _views.Count; i++)
            {
                int column = i % columnCount;
                int row = i / columnCount;
                Transform instanceTransform = _views[i].Instance.transform;
                instanceTransform.localPosition = new Vector3(column * spacing.x, row * spacing.y, 0f);
                instanceTransform.localRotation = Quaternion.identity;
                instanceTransform.localScale = Vector3.one;
            }
        }

        private sealed class ItemModelView
        {
            public ItemModelView(ItemData itemData, GameObject instance)
            {
                ItemData = itemData;
                Instance = instance;
            }

            public ItemData ItemData { get; }
            public GameObject Instance { get; }
        }
    }
}
