using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterAttr : MonoBehaviour, IPunObservable,IPunInstantiateMagicCallback
{
    [SerializeField]private GameObject model_Surface;
    public int teamCode;

    // 属性
    public float health = 100f;
    public float moveSpeed = 200f;
    public float jumpHeight = 5f;
    public int money = 500;

    private void Start()
    {
        if (teamCode == 0)
            model_Surface.GetComponent<SkinnedMeshRenderer>().material.color = Color.blue / 2;
        else
            model_Surface.GetComponent<SkinnedMeshRenderer>().material.color = Color.red / 2;
    }

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
            stream.SendNext(money);
            //stream.SendNext(teamCode);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            moveSpeed = (float)stream.ReceiveNext();
            jumpHeight = (float)stream.ReceiveNext();
            money = (int)stream.ReceiveNext();           
        }

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        teamCode = (int)info.photonView.InstantiationData[0];
    }
}
