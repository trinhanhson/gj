using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor.Rendering;
using UnityEngine;

public class Fart : MonoBehaviour
{
    public Rigidbody rb;

    public PhotonView photonView;

    public float speed;

    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }
}
