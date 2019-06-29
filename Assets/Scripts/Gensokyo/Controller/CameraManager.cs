using System;
using UnityEngine;
using Utilities;

namespace Gensokyo.Controller
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private float followSpeed = 3f;
        [SerializeField] private float transitionSpeed = 15f;

        private Transform _target;

        public void Init(Transform t)
        {
            _target = t;
        }

        private void FixedUpdate()
        {
            // TODO: Area locking / changing
            var speed = Time.fixedDeltaTime * followSpeed;
            var targetPosition = Vector3.Lerp(transform.position, _target.position, speed);
            transform.position = targetPosition;
        }
    }
}
