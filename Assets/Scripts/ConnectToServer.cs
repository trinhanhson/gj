using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject nameGO;

    public TextMeshProUGUI nameText;

    public void Startgame()
    {
        if (nameText.text != "")
        {
            PhotonNetwork.LocalPlayer.NickName = nameText.text;

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !nameGO.activeInHierarchy)
        {
            nameGO.SetActive(true);
        }
    }

    // private void Start()
    // {
    //     PhotonNetwork.ConnectUsingSettings();
    // }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
