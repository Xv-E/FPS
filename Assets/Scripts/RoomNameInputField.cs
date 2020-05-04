using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;


namespace Com.MyCompany.MyGame
{
    
    /// <summary>
    /// Player name input field. Let the user input his name, will appear above the player in the game.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class RoomButton : MonoBehaviour
    {
        #region Private Constants

        #endregion


        #region MonoBehaviour CallBacks
        private void Start()
        {
            Create();
        }


        #endregion


        #region Public Methods

        public void Create()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                RoomOptions options = new RoomOptions { MaxPlayers = 4 };
                PhotonNetwork.JoinOrCreateRoom("name", options, null);

                //PhotonNetwork.JoinRoom("123");
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = Launcher.gameVersion; ;
            }
        }





        #endregion
    }
}
