using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
namespace Com.MyCompany.MyGame
{
    public class RoomListManager : MonoBehaviourPunCallbacks
    {
        public GameObject roomNamePrefab;
        public Transform gridLayout;

        private void Start()
        {
            
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("roomupdate run");
            for (int i = 0; i < gridLayout.childCount; i++)
            {
                if (gridLayout.GetChild(i).gameObject.GetComponentInChildren<Text>().text == roomList[i].Name)
                {
                    Destroy(gridLayout.GetChild(i).gameObject);
                }
            }
            foreach (var room in roomList)
            {
                GameObject newRoom = Instantiate(roomNamePrefab, gridLayout.position, Quaternion.identity);
                newRoom.GetComponentInChildren<Text>().text = room.Name;
                newRoom.transform.SetParent(gridLayout);
            }
        }
    }
}
