using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{

    public class NewRoomButton : MonoBehaviour
    {
        private void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(Create);
        }

        public void Create()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                RoomOptions options = new RoomOptions { MaxPlayers = 4 };
                PhotonNetwork.JoinOrCreateRoom(this.GetComponentInChildren<Text>().text, options, null);

                //PhotonNetwork.JoinRoom("123");
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = Launcher.gameVersion;
            }
        }


    }



}

