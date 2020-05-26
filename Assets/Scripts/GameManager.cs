using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields
        public static GameManager Instance;
        [Tooltip("The prefab to use for representing the player")]
        public GameObject[] playerPrefab;
        public Transform red_Home;
        public Transform blue_Home;
        #endregion

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            Instance = this;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.localPlayer == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    int teamCode = PlayerPrefs.GetInt("team");
                    int characterCode = PlayerPrefs.GetInt("character");
                    //PlayerPrefs.DeleteKey("team");
                    Transform home;
                    object[] data = {teamCode};
                    // 设置出生点
                    if (teamCode == 0)
                        home = blue_Home;
                    else
                        home = red_Home;
                    PhotonNetwork.Instantiate(this.playerPrefab[characterCode].name, home.position, home.rotation, 0, data);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        
        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            if(PlayerManager.localPlayer!=null)
                PhotonNetwork.Destroy(PlayerManager.localPlayer);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }


        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }


        #endregion
    }
}
