using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
    }
}
