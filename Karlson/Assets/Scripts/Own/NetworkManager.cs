using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void MenuJoinCreateButton()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnConnectedToMaster()
    {
        UnityEngine.Debug.Log("Connected to master");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        UnityEngine.Debug.Log("Connected to master");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

}
