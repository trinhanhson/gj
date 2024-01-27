using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Run,
        Win,
        Attack,
        Hit
    }

    public State state;

    public Animator animator;

    public float speed;

    public PhotonView photonView;

    public Rigidbody rb;

    public Rigidbody midSpine;

    public Collider hitbox;

    public Vector3 inputDir;

    public float rotationSpeed;

    public bool ableToAttack = true;

    public bool isDie = false;

    public Rigidbody[] rigidbodies;

    public Collider[] colliders;

    private void Start()
    {
        if (photonView.IsMine)
        {
            Manager.Instance.player = this;

            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else
        {
            Manager.Instance.enemy = this;

            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        rigidbodies = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Rigidbody>();

        colliders = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Collider>();

        foreach (var i in rigidbodies)
        {
            i.isKinematic = true;
        }

        foreach (var i in colliders)
        {
            i.enabled = false;
        }
    }

    void Update()
    {
        if (isDie)
        {
            return;
        }
        if (photonView.IsMine)
        {
            switch (state)
            {
                case State.Idle:
                    if (transform.position.y < Manager.Instance.bottom.position.y)
                    {
                        isDie = true;
                        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                        {
                            if (!PhotonNetwork.PlayerList[i].Equals(PhotonNetwork.LocalPlayer))
                            {
                                photonView.RPC("RPC_Finish", RpcTarget.AllViaServer, PhotonNetwork.PlayerList[i]);
                            }

                        }
                    }

                    break;
                case State.Run:
                    if (transform.position.y < Manager.Instance.bottom.position.y)
                    {
                        isDie = true;
                        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                        {
                            if (!PhotonNetwork.PlayerList[i].Equals(PhotonNetwork.LocalPlayer))
                            {
                                photonView.RPC("RPC_Finish", RpcTarget.AllViaServer, PhotonNetwork.PlayerList[i]);
                            }
                        }
                    }

                    break;
                case State.Attack:
                    if (transform.position.y < Manager.Instance.bottom.position.y)
                    {
                        isDie = true;
                        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                        {
                            if (!PhotonNetwork.PlayerList[i].Equals(PhotonNetwork.LocalPlayer))
                            {
                                photonView.RPC("RPC_Finish", RpcTarget.AllViaServer, PhotonNetwork.PlayerList[i]);
                            }
                        }
                    }
                    break;
                case State.Win:
                    break;
                default:
                    break;
            }
        }
    }

    [PunRPC]
    public void RPC_Finish(Player player)
    {
        Debug.Log(1);
        Manager.Instance.Finish(player);
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            inputDir = new Vector3(Manager.Instance.joystick.Direction.x, 0, Manager.Instance.joystick.Direction.y);

            transform.forward = Vector3.Slerp(transform.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

            Vector3 temp = speed * Time.fixedDeltaTime * inputDir;
            switch (state)
            {
                case State.Idle:
                    rb.MovePosition(rb.position + temp);

                    if (temp.sqrMagnitude > 0)
                    {
                        ChangeState(State.Run);
                    }

                    break;
                case State.Run:
                    rb.MovePosition(rb.position + temp);

                    if (temp.sqrMagnitude <= 0)
                    {
                        ChangeState(State.Idle);
                    }

                    break;
                case State.Attack:
                    break;
                case State.Win:
                    break;
                default:
                    break;
            }
        }
    }

    public void Push(Vector3 force)
    {
        photonView.RPC("RPC_Push", RpcTarget.AllViaServer, force);
    }

    [PunRPC]
    public void RPC_Push(Vector3 force)
    {
        ChangeState(State.Hit);

        midSpine.AddForce(force + Vector3.up * 2, ForceMode.VelocityChange);
    }

    public void ChangeState(State _state)
    {
        state = _state;

        switch (state)
        {
            case State.Idle:
                animator.enabled = true;
                foreach (var i in rigidbodies)
                {
                    i.isKinematic = true;
                }

                foreach (var i in colliders)
                {
                    i.enabled = false;
                }

                rb.isKinematic = false;

                hitbox.enabled = true;
                animator.SetTrigger("Idle");
                break;
            case State.Run:
                animator.SetTrigger("Run");
                break;
            case State.Attack:
                animator.SetTrigger("Attack");
                ableToAttack = false;
                DOVirtual.DelayedCall(2, () => { ChangeState(State.Idle); ableToAttack = true; });
                break;
            case State.Win:
                animator.SetTrigger("Win");
                break;
            case State.Hit:
                animator.enabled = false;
                foreach (var i in rigidbodies)
                {
                    i.isKinematic = false;
                }

                foreach (var i in colliders)
                {
                    i.enabled = true;
                }

                rb.isKinematic = true;

                hitbox.enabled = false;

                DOVirtual.DelayedCall(2f, () =>
                {
                    animator.enabled = true;
                    foreach (var i in rigidbodies)
                    {
                        i.isKinematic = true;
                    }

                    foreach (var i in colliders)
                    {
                        i.enabled = false;
                    }

                    rb.isKinematic = false;

                    hitbox.enabled = true;

                    ChangeState(State.Idle);
                });

                break;
            default:
                break;
        }
    }

    public void Restart()
    {
        photonView.RPC("RPC_Restart", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_Restart()
    {
        Manager.Instance.RPC_Restart();
    }
}
