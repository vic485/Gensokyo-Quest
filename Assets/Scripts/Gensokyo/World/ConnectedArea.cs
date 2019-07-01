using System;
using Gensokyo.Controller;
using UnityEngine;

namespace Gensokyo.World
{
    public class ConnectedArea : Room
    {
        // TODO: Can we make an easy way to setup / adjust the room bounds?
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            StartCoroutine(CameraManager.Instance.ChangeRoom(this));
        }
    }
}