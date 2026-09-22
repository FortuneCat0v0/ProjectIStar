using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

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

        private readonly List<GameObject> _instances = new();
        private int _desiredCount = -1;
        private bool _loadFailed;

        private void Update()
        {
            int currentCount = CountMatchingItems(GameDataManager.Instance.ItemDataList);
            if (currentCount == _desiredCount)
            {
                return;
            }

            _desiredCount = currentCount;
            if (!_loadFailed)
            {
                Synchronize(currentCount);
            }
        }

        private void Synchronize(int desiredCount)
        {
            while (_instances.Count > desiredCount)
            {
                RemoveLastInstance();
            }

            if (_instances.Count == desiredCount)
            {
                return;
            }

            ItemRow itemRow = ConfigSystem.Instance.Tables.TbItem.GetOrDefault(itemId);
            if (itemRow == null || string.IsNullOrWhiteSpace(itemRow.Actor))
            {
                Log.Error($"ItemModelList '{name}' cannot find a model address for item {itemId}.");
                _loadFailed = true;
                return;
            }

            if (!GameModule.Resource.CheckLocationValid(itemRow.Actor))
            {
                Log.Error($"ItemModelList '{name}' model address '{itemRow.Actor}' is invalid.");
                _loadFailed = true;
                return;
            }

            while (_instances.Count < desiredCount)
            {
                GameObject instance = GameModule.Resource.LoadGameObject(itemRow.Actor, transform);
                if (instance == null)
                {
                    Log.Error($"ItemModelList '{name}' failed to load '{itemRow.Actor}'.");
                    _loadFailed = true;
                    break;
                }

                instance.name = $"{itemRow.Actor}_{_instances.Count + 1}";
                _instances.Add(instance);
            }

            ArrangeInstances();
        }

        private int CountMatchingItems(IReadOnlyList<ItemData> items)
        {
            int count = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].itemId == itemId)
                {
                    count++;
                }
            }

            return count;
        }

        private void RemoveLastInstance()
        {
            int lastIndex = _instances.Count - 1;
            GameObject instance = _instances[lastIndex];
            _instances.RemoveAt(lastIndex);
            if (instance != null)
            {
                Destroy(instance);
            }

            ArrangeInstances();
        }

        private void ArrangeInstances()
        {
            int columnCount = Mathf.Max(1, columns);
            for (int i = 0; i < _instances.Count; i++)
            {
                int column = i % columnCount;
                int row = i / columnCount;
                Transform instanceTransform = _instances[i].transform;
                instanceTransform.localPosition = new Vector3(column * spacing.x, row * spacing.y, 0f);
                instanceTransform.localRotation = Quaternion.identity;
                instanceTransform.localScale = Vector3.one;
            }
        }
    }
}
