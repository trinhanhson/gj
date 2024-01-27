using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LimbCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            PlayerController.Instance.isGrounded = true;
        }
    }
}