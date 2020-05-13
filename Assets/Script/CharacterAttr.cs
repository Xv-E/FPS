using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterAttr : MonoBehaviour, IPunObservable
{
    // 属性
    public float health = 100f;
    public float moveSpeed = 200f;
    public float jumpHeight = 5f;

    void FixedUpdate()
    {

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(moveSpeed);
            stream.SendNext(jumpHeight);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            moveSpeed = (float)stream.ReceiveNext();
            jumpHeight = (float)stream.ReceiveNext();
        }
    }
}
