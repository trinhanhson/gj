using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public float speed;

    public PhotonView photonView;

    public Rigidbody rb;

    private void Start()
    {
        if (photonView.IsMine)
        {
            Manager.Instance.player = this;
        }
        else
        {
            Manager.Instance.enemy = this;
        }
    }

    // void Update()
    // {
    //     if (photonView.IsMine)
    //     {
    //         // Vector2 moveVector = Vector2.up * Input.GetAxis("Vertical") + Vector2.right * Input.GetAxis("Horizontal");

    //         transform.Translate(speed * Time.deltaTime * new Vector3(Manager.Instance.joystick.Direction.x, 0, Manager.Instance.joystick.Direction.y));
    //     }
    // }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.AddForce(speed * Time.fixedDeltaTime * new Vector3(Manager.Instance.joystick.Direction.x, 0, Manager.Instance.joystick.Direction.y), ForceMode.VelocityChange);
        }
    }
}
