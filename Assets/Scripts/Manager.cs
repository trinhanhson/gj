using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public GameObject playerPrefab1;

    public GameObject playerPrefab2;

    public GameObject fartPrefab;

    public Joystick joystick;

    public Button attackButton;

    public Transform pos1, pos2;

    public Transform bottom;

    public SimpleController player;

    public SimpleController enemy;

    public GameObject finishUI;

    public TextMeshProUGUI winText;

    public Button continueButton;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab1.name, pos1.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab2.name, pos2.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { Attack(); }
    }

    public void Attack()
    {
        if (!player.ableToAttack)
        {
            return;
        }

        player.ChangeState(SimpleController.State.Attack);

        DOVirtual.DelayedCall(1, () => PhotonNetwork.Instantiate(fartPrefab.name, new Vector3(player.transform.position.x, 0.5f, player.transform.position.z), Quaternion.identity).transform.forward = new Vector3(player.transform.forward.x, 0, player.transform.forward.z));
    }

    public void Finish(Player player)
    {
        finishUI.SetActive(true);

        winText.text = player.NickName + " win!";

        if (PhotonNetwork.IsMasterClient)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        finishUI.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            player.transform.SetPositionAndRotation(pos1.position, Quaternion.identity);
        }
        else
        {
            player.transform.SetPositionAndRotation(pos2.position, Quaternion.identity);
        }

        player.ChangeState(SimpleController.State.Idle);
    }
}
