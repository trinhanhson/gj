using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI createText;

    [SerializeField] TextMeshProUGUI joinText;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new()
        {
            MaxPlayers = 2
        };

        PhotonNetwork.CreateRoom(createText.text, roomOptions, null);

        // PhotonNetwork.CreateRoom(createText.text, null, null);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinText.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GamePlay");
    }
}
