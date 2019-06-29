using System;
using UnityEngine;

namespace Gensokyo.Controller
{
    public class Player : MonoBehaviour
    {
        [Header("Stats")] public float moveSpeed = 3.5f;

        [Header("States")] public int idleHorizontal;
        public int idleVertical;

        private float _horizontal;
        private float _vertical;

        private Animator _animator;
        private Rigidbody2D _rigidbody;

        #region Animator Hashes

        private static readonly int IdleVerticalHash = Animator.StringToHash("IdleVertical");
        private static readonly int IdleHorizontalHash = Animator.StringToHash("IdleHorizontal");
        private static readonly int VerticalHash = Animator.StringToHash("Vertical");
        private static readonly int HorizontalHash = Animator.StringToHash("Horizontal");
        private static readonly int MovingHash = Animator.StringToHash("Moving");

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.applyRootMotion = false;
            _animator.SetInteger(IdleVerticalHash, -1);

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.drag = 4f;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            CameraManager.Instance.Init(transform);
        }

        private void Update()
        {
            // TODO: Custom input system
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            var moveDir = new Vector2(_horizontal, _vertical);
            var moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));

            _rigidbody.drag = moveAmount > 0 ? 0f : 4f;
            _animator.SetBool(MovingHash, Mathf.Approximately(_rigidbody.drag, 0f));
            _rigidbody.velocity = moveDir * (moveAmount * moveSpeed);
            
            HandleMovementAnimations();
            HandleIdleAnimations();
        }

        #endregion

        private void HandleMovementAnimations()
        {
            _animator.SetFloat(HorizontalHash, _horizontal, 0.15f, Time.fixedDeltaTime);
            _animator.SetFloat(VerticalHash, _vertical, 0.15f, Time.fixedDeltaTime);
        }
        
        private void HandleIdleAnimations()
        {
            // Do nothing if we are at idle
            if (Mathf.Approximately(_horizontal, 0f) && Mathf.Approximately(_vertical, 0f))
                return;

            // Are we moving horizontally?
            if (!Mathf.Approximately(_horizontal, 0f))
                idleHorizontal = _horizontal > 0 ? 1 : -1;
            else
                idleHorizontal = 0;

            // Are we moving vertically?
            if (!Mathf.Approximately(_vertical, 0f))
                idleVertical = _vertical > 0 ? 1 : -1;
            else
                idleVertical = 0;
            
            _animator.SetInteger("IdleHorizontal", idleHorizontal);
            _animator.SetInteger("IdleVertical", idleVertical);
        }
    }
}
