using System;
using System.Collections;
using Gensokyo.World;
using UnityEngine;
using Utilities;

namespace Gensokyo.Controller
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private float followSpeed = 3f;
        [SerializeField] private float transitionSpeed = 15f;
        public bool Changing { get; private set; }

        private Transform _target;
        private Room _currentRoom;
        private readonly Vector2 _offset = new Vector2(8.906f, 5f);
        private float _delta;
        private bool _init;

        public void Init(Transform t)
        {
            _target = t;
            _init = true;
        }

        private void FixedUpdate()
        {
            _delta = Time.fixedDeltaTime;

            if (Changing || !_init)
                return;
            
            var speed = Time.fixedDeltaTime * followSpeed;
            var targetPosition = Vector3.Lerp(transform.position, _target.position, speed);
            transform.position = targetPosition;
        }

        private Vector3 LockToRoom(Vector3 targetPos)
        {
            // Check left / right movement
            if (targetPos.x < _currentRoom.leftBound + _offset.x)
                targetPos.x = _currentRoom.leftBound + _offset.x;
            else if (targetPos.x > _currentRoom.rightBound - _offset.x)
                targetPos.x = _currentRoom.rightBound - _offset.x;
            
            // Check up / down movement
            if (targetPos.y < _currentRoom.bottomBound + _offset.y)
                targetPos.y = _currentRoom.bottomBound + _offset.y;
            else if (targetPos.y > _currentRoom.topBound - _offset.y)
                targetPos.y = _currentRoom.topBound - _offset.y;

            return targetPos;
        }

        public IEnumerator ChangeRoom(Room room)
        {
            Changing = true;
            _currentRoom = room;

            if (room is SingularArea)
            {
                Changing = false;
                yield break;
            }

            var speed = _delta * transitionSpeed;
            var targetPosition = LockToRoom(transform.position);
            var waitTime = new WaitForSeconds(_delta); // This is fine because we wait a fixed tick?
            
            // Move until we are within the new room
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
                yield return waitTime;
            }

            Changing = false;
        }
    }
}
