using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame {
public class BulletManager : MonoBehaviourPunCallbacks, IPunObservable
    {

        public float damage;
        public GameObject from_player;
        public bool isDestroy = false;
        private GameObject metalParticle;
        private GameObject explodeParticle;
        private GameObject bloodParticle;
        private GameObject concreteParticle;
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
            metalParticle = Resources.Load("Particle/Metal Impact Prefab") as GameObject;
            explodeParticle = Resources.Load("Particle/Explosion Prefab") as GameObject;
            bloodParticle = Resources.Load("Particle/Blood Impact Prefab") as GameObject;
            concreteParticle = Resources.Load("Particle/Concrete Impact Prefab") as GameObject;
            Invoke("setDestroy", 5f);
            if(this.gameObject.tag.Equals("Rocket"))
            {
                Invoke("boom", 4f);
            }
            //if(photonView.IsMine)
            //{
            //    Invoke("setDestroy", 5f);
            //}
        }

        void setDestroy()
        {
            isDestroy = true;
        }
        void boom()
        {
            GameObject particle = Instantiate(explodeParticle, this.transform.position, this.transform.rotation);
            Destroy(particle, 0.8f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isDestroy)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(this.gameObject.tag.Equals("Rocket"))
            {
                GameObject particle = Instantiate(explodeParticle, this.transform.position, this.transform.rotation);
                particle.GetComponent<ExplodeCollider>().fromplayer = this.from_player;
                particle.GetComponentInChildren<ExplodeCollider>().fromplayer = this.from_player;
                Destroy(particle, 1f);
                Destroy(this.gameObject, 0.5f);
                Destroy(particle, 2f);
            }
            // 8是character层
            if (other.gameObject.layer == 8) {
                other.GetComponent<CharacterAttr>().health -= damage;
                GameObject particle = Instantiate(bloodParticle, this.transform.position, this.transform.rotation);
                if (other.GetComponent<CharacterAttr>().health < 0)
                {
                    if(other.GetComponent<PhotonView>().IsMine)
                    {
                        Cursor.visible = true;
                        //Debug.Log("create leave button");
                        GameObject leaveButton = Instantiate(Resources.Load("Particle/LeaveCanvas") as GameObject, this.transform.position, this.transform.rotation);
                    }
                    PhotonNetwork.Destroy(other.gameObject);
                    from_player.GetComponent<CharacterAttr>().money += 200;
                }
            }
            //9为ground,11为wall
            if(other.gameObject.layer == 11|| other.gameObject.layer == 9)
            {
                GameObject particle = Instantiate(concreteParticle, this.transform.position, this.transform.rotation);
                Destroy(particle, 3f);
            }
            if(other.GetComponent<Rigidbody>())
            {
                //获得该方向的力
                other.GetComponent<Rigidbody>().AddForce( (other.transform.position- this.transform.position) * 200);
                GameObject particle = Instantiate(metalParticle, this.transform.position, this.transform.rotation);
                Destroy(particle, 3f);
            }

            // 与子弹碰撞不消失
            if (other.gameObject.layer!=9)
                isDestroy = true;
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
