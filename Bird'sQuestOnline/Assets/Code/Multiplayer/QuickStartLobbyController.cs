using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject quickStartButton;
    [SerializeField] private GameObject quickCancelButton;
    [SerializeField] private GameObject loadingButton;
    [SerializeField] private int RoomSize = 0;

    // BUTTONS ////////////////
    public void OnQuickStart()
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick Start");
    }

    public void OnQuickCancel()
    {
        quickStartButton.SetActive(true);
        quickCancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
        Debug.Log("Quick Cancel");
    }
    //////////////////////////////

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
        loadingButton.SetActive(false);
    }

    private void CreateRoom()
    {
        Debug.Log("Creating room");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room " + randomRoomNumber, roomOps);
        Debug.Log("Room " + randomRoomNumber);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... retrying");
        CreateRoom();
    }
}
