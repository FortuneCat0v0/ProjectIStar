using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 4f;
        [SerializeField] private Animator animator;

        private Rigidbody body;
        private Vector2 moveInput;
        private Vector2 facing = Vector2.down;

        private static readonly int XHash = Animator.StringToHash("X");
        private static readonly int YHash = Animator.StringToHash("Y");
        private static readonly int VelocityHash = Animator.StringToHash("Velocity");

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotation;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            if (animator == null) animator = GetComponentInChildren<Animator>();
            UpdateAnimationParameters();
        }

        private void Update()
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

            if (moveInput.sqrMagnitude > 0f)
            {
                facing = Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)
                    ? new Vector2(Mathf.Sign(moveInput.x), 0f)
                    : new Vector2(0f, Mathf.Sign(moveInput.y));
            }
            UpdateAnimationParameters();
        }

        private void FixedUpdate()
        {
            Vector3 velocity = body.velocity;
            velocity.x = moveInput.x * moveSpeed;
            velocity.z = moveInput.y * moveSpeed;
            body.velocity = velocity;
        }

        private void UpdateAnimationParameters()
        {
            if (animator == null || animator.runtimeAnimatorController == null) return;

            animator.SetFloat(XHash, facing.x);
            animator.SetFloat(YHash, facing.y);
            animator.SetFloat(VelocityHash, moveInput.magnitude);
        }
    }
}
