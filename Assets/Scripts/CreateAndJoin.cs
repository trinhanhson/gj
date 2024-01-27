using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI createText;

    [SerializeField] TextMeshProUGUI joinText;

    public Transform playerList;

    public GameObject playerEntryPrefab;

    public GameObject inRoomPanel;

    public GameObject outRoomPanel;

    public Dictionary<int, GameObject> playerListEntries;

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

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playerEntryPrefab);
            entry.transform.SetParent(playerList);
            entry.transform.localScale = Vector3.one;
            entry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = p.NickName;

            playerListEntries.Add(p.ActorNumber, entry);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(playerEntryPrefab);
        entry.transform.SetParent(playerList);
        entry.transform.localScale = Vector3.one;
        entry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newPlayer.NickName;

        playerListEntries.Add(newPlayer.ActorNumber, entry);

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 2)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }
}
