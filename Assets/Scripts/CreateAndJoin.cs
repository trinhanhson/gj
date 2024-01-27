using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI createText;

    [SerializeField] TextMeshProUGUI joinText;

    public Transform playerList;

    public GameObject inRoomPanel;

    public GameObject outRoomPanel;

    public Dictionary<int, GameObject> playerListEntries;

    public List<GameObject> playerEntrys = new();

    public Button startButton;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new()
        {
            MaxPlayers = 2
        };

        PhotonNetwork.CreateRoom(createText.text, roomOptions, null);

        // PhotonNetwork.CreateRoom(createText.text, null, null);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        PhotonNetwork.AutomaticallySyncScene = true;

        startButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("GamePlay");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created sucefully");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinText.text);
    }

    public override void OnJoinedRoom()
    {
        outRoomPanel.SetActive(false);

        inRoomPanel.SetActive(true);

        startButton.gameObject.SetActive(false);

        playerListEntries = new Dictionary<int, GameObject>();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerEntrys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player " + (i + 1) + ":\n" + PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerEntrys[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player " + 2 + ":\n" + PhotonNetwork.PlayerList[1].NickName;

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 2)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        outRoomPanel.SetActive(true);

        inRoomPanel.SetActive(false);
        
        playerEntrys[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player 1:";

        playerEntrys[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player 2:";
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            LeaveRoom();
        }
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
    
    public override  void OnLeftLobby()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
