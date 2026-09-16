using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 当前场景内的通用传送门。通过门 ID 配对，并同步平移玩家与主摄像机。
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class SceneDoor : MonoBehaviour
    {
        [Header("Door Link")]
        [SerializeField] private string doorId;
        [SerializeField] private string targetDoorId;
        [SerializeField] private Transform cameraRoot;

        [Header("Interaction")]
        [SerializeField] private KeyCode interactKey = KeyCode.F;
        [SerializeField] private Vector3 arrivalOffset = new Vector3(0f, 0f, -1f);

        private static readonly Dictionary<string, SceneDoor> Doors = new();
        private static bool _isTeleporting;

        private Transform _playerInRange;

        private void Awake()
        {
            foreach (Collider doorCollider in GetComponents<Collider>())
            {
                if (doorCollider.isTrigger)
                {
                    return;
                }
            }

            Log.Error($"SceneDoor '{name}' requires a trigger Collider.");
        }

        private void OnEnable()
        {
            if (string.IsNullOrWhiteSpace(doorId))
            {
                Log.Error($"SceneDoor '{name}' requires a unique doorId.");
                return;
            }

            if (Doors.TryGetValue(doorId, out SceneDoor existingDoor) && existingDoor != this)
            {
                Log.Error($"Duplicate SceneDoor id '{doorId}'. Door ids must be unique in the scene.");
                return;
            }

            Doors[doorId] = this;
        }

        private void OnDisable()
        {
            if (Doors.TryGetValue(doorId, out SceneDoor existingDoor) && existingDoor == this)
            {
                Doors.Remove(doorId);
            }

            _playerInRange = null;
        }

        private void Update()
        {
            if (_playerInRange == null || _isTeleporting || !Input.GetKeyDown(interactKey))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(targetDoorId) || !Doors.TryGetValue(targetDoorId, out SceneDoor targetDoor))
            {
                Log.Error($"SceneDoor '{name}' could not find target door '{targetDoorId}'.");
                return;
            }

            Teleport(_playerInRange, targetDoor.GetArrivalPosition(), targetDoor.cameraRoot);
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

        private Vector3 GetArrivalPosition()
        {
            return transform.TransformPoint(arrivalOffset);
        }

        private static Transform GetPlayerRoot(Collider other)
        {
            Transform candidate = other.attachedRigidbody != null
                ? other.attachedRigidbody.transform
                : other.transform.root;
            return candidate.CompareTag("Player") ? candidate : null;
        }

        private static void Teleport(Transform player, Vector3 destination, Transform targetCameraRoot)
        {
            _isTeleporting = true;

            Camera mainCamera = Camera.main;
            if (mainCamera != null && targetCameraRoot != null)
            {
                Transform cameraTransform = mainCamera.transform;
                cameraTransform.SetParent(targetCameraRoot, false);
                cameraTransform.localPosition = Vector3.zero;
                cameraTransform.localRotation = Quaternion.identity;
                cameraTransform.localScale = Vector3.one;
            }
            else if (mainCamera == null)
            {
                Log.Warning("SceneDoor could not find a camera tagged MainCamera.");
            }
            else
            {
                Log.Warning("SceneDoor target has no CameraRoot configured.");
            }

            if (player.TryGetComponent(out Rigidbody body))
            {
                body.position = destination;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
            else
            {
                player.position = destination;
            }

            Physics.SyncTransforms();
            _isTeleporting = false;
        }
    }
}
