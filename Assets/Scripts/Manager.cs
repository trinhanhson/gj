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

        Initialize();
    }

    public void Update()
    {
        if (!isTimerRunning) return;

        float countdown = TimeRemaining();
        timeTextMesh.text = string.Format("Game starts in {0} seconds", countdown.ToString("n0"));

        if (countdown > 0.0f) return;

        OnTimerEnds();
    }

    private void Initialize()
    {
        if (TryGetStartTime(out int propStartTime))
        {
            startTime = propStartTime;
            Debug.Log("Initialize sets StartTime " + this.startTime + " server time now: " + PhotonNetwork.ServerTimestamp + " remain: " + TimeRemaining());


            isTimerRunning = TimeRemaining() > 0;

            if (isTimerRunning)
                OnTimerRuns();
            else
                OnTimerEnds();
        }
    }

    public static bool TryGetStartTime(out int startTimestamp)
    {
        startTimestamp = PhotonNetwork.ServerTimestamp;

        object startTimeFromProps;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("StartTime", out startTimeFromProps))
        {
            startTimestamp = (int)startTimeFromProps;
            return true;
        }

        return false;
    }

    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - startTime;
        return Countdown - timer / 1000f;
    }


    private void OnTimerRuns()
    {
        isTimerRunning = true;
    }

    private void OnTimerEnds()
    {
        isTimerRunning = false;

        timeTextMesh.text = string.Empty;

        StartGame();
    }

    private void StartGame()
    {

    }
}
