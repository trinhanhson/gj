using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public GameObject playerPrefab;

    public GameObject fartPrefab;

    public Joystick joystick;

    public Button attackButton;

    public float minX, maxX, minZ, maxZ;

    public SimpleController player;

    public SimpleController enemy;

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
        Vector3 randomPosition = new(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));

        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { Attack(); }
    }

    public void Attack()
    {
        PhotonNetwork.Instantiate(fartPrefab.name, new Vector3(player.transform.position.x, 0.5f, player.transform.position.z), Quaternion.identity).transform.forward = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
    }
}
