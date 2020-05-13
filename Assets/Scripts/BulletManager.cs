using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.MyCompany.MyGame {
public class BulletManager : MonoBehaviourPunCallbacks, IPunObservable,IPunInstantiateMagicCallback
    {

        public float damage;
        public GameObject from_player;
        public bool isDestroy = false;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting && PhotonNetwork.IsConnected)
            {
                stream.SendNext(damage);
                stream.SendNext(isDestroy);
            }
            else
            {
                damage = (float)stream.ReceiveNext();
                isDestroy = (bool)stream.ReceiveNext();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Invoke("setDestroy", 5f);
            //if(photonView.IsMine)
            //{
            //    Invoke("setDestroy", 5f);
            //}
        }

        void setDestroy()
        {
            isDestroy = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isDestroy)
            {
                Destroy(this.gameObject);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            // 8是character层
            if (other.gameObject.layer == 8) {
                other.GetComponent<CharacterAttr>().health -= damage;
            }
            // 与子弹碰撞不消失
            if (other.gameObject.layer!=9)
                isDestroy = true;
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (isDestroy)
            {
                Destroy(this.gameObject);
            }
        }

        //void OnTriggerStay(Collider other)
        //{
        //    // we dont' do anything if we are not the local player.

        //    // We are only interested in Beamers
        //    // we should be using tags but for the sake of distribution, let's simply check by name.
        //    if (other.name.Contains("Robot"))
        //    {
        //            Debug.Log("is robot");
        //        if (other.GetComponent<PhotonView>().IsMine)
        //        {
        //                Debug.Log("ismine return");
        //            return;
        //        }
        //            Debug.Log("health");
        //            other.GetComponent<PlayerManager>().reduceHealeh();
        //         //other.GetComponent<PlayerManager>().Health -= damage * Time.deltaTime;
        //    }
        //    // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.

        //}


    }
}
