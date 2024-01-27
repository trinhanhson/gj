using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public float speed;

    public PhotonView photonView;

    private void Start()
    {
        if (photonView.IsMine)
        {

        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Vector2 moveVector = Vector2.up * Input.GetAxis("Vertical") + Vector2.right * Input.GetAxis("Horizontal");

            transform.Translate(speed * Time.deltaTime * Manager.Instance.joystick.Direction);
        }
    }
}
