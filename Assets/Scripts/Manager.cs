using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public GameObject playerPrefab;

    public Joystick joystick;

    public float minX, maxX, minY, maxY;

    public int playerCount;

    public TextMeshProUGUI timeTextMesh;

    public float Countdown = 5.0f;

    public SimpleController player;

    public SimpleController enemy;

    private bool isTimerRunning;

    private int startTime;

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
        Vector2 randomPosition = new(Random.Range(minX, maxX), Random.Range(minY, maxY));

        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
