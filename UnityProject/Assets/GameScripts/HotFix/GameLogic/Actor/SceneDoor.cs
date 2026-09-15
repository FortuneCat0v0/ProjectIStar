using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    /// <summary>
    /// 可配置的场景门。玩家进入触发范围后按交互键，切换到目标场景的目标门前。
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class SceneDoor : MonoBehaviour
    {
        [Header("Door Link")]
        [SerializeField] private string doorId;
        [SerializeField] private string targetScene;
        [SerializeField] private string targetDoorId;

        [Header("Interaction")]
        [SerializeField] private KeyCode interactKey = KeyCode.F;
        [SerializeField] private Vector3 arrivalOffset = new Vector3(0f, 0f, -1f);

        private static Transform _transitioningPlayer;
        private static string _pendingDoorId;
        private static bool _isLoading;

        private Transform _playerInRange;

        private void Awake()
        {
            Collider[] colliders = GetComponents<Collider>();
            bool hasTrigger = false;
            foreach (Collider doorCollider in colliders)
            {
                if (doorCollider.isTrigger)
                {
                    hasTrigger = true;
                    break;
                }
            }

            if (!hasTrigger)
            {
                Log.Error($"SceneDoor '{name}' requires a trigger Collider.");
            }
        }

        private void OnEnable()
        {
            TryPlaceArrivingPlayer();
        }

        private void Update()
        {
            if (_playerInRange == null || _isLoading || !Input.GetKeyDown(interactKey))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(targetScene) || string.IsNullOrWhiteSpace(targetDoorId))
            {
                Log.Error($"SceneDoor '{name}' has an incomplete destination configuration.");
                return;
            }

            LoadDestinationAsync(_playerInRange).Forget();
        }

        private void OnTriggerEnter(Collider other)
        {
            Transform player = GetPlayerRoot(other);
            if (player != null)
            {
                _playerInRange = player;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Transform player = GetPlayerRoot(other);
            if (player == _playerInRange)
            {
                _playerInRange = null;
            }
        }

        private static Transform GetPlayerRoot(Collider other)
        {
            Transform candidate = other.attachedRigidbody != null
                ? other.attachedRigidbody.transform
                : other.transform.root;
            return candidate.CompareTag("Player") ? candidate : null;
        }

        private async UniTaskVoid LoadDestinationAsync(Transform player)
        {
            _isLoading = true;
            _transitioningPlayer = player;
            _pendingDoorId = targetDoorId;
            DontDestroyOnLoad(player.gameObject);

            try
            {
                await GameModule.Scene.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void TryPlaceArrivingPlayer()
        {
            if (_transitioningPlayer == null || string.IsNullOrEmpty(_pendingDoorId) || doorId != _pendingDoorId)
            {
                return;
            }

            Vector3 destination = transform.TransformPoint(arrivalOffset);
            if (_transitioningPlayer.TryGetComponent(out Rigidbody body))
            {
                body.position = destination;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
            else
            {
                _transitioningPlayer.position = destination;
            }

            // _transitioningPlayer.rotation = transform.rotation;
            _transitioningPlayer = null;
            _pendingDoorId = null;
        }
    }
}
