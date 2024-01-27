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

    public float force;

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(20);

        PhotonNetwork.Destroy(photonView);
    }

    private void FixedUpdate()
    {
        if (photonView)
        {
            rb.velocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<SimpleController>().Push(force * transform.forward);

            PhotonNetwork.Destroy(photonView);
        }
    }
}
