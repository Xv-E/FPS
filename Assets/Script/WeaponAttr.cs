using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponAttr : MonoBehaviour, IPunObservable
{
    public GameObject frontHandler;
    public GameObject backHandler;

    public float damage;      // 攻击力
    public float bulletSpeed; // 子弹速度
    public int shootSpeed;  // 射速
    public int bulletNum;   // 装弹量
    public int currentBulletNum; // 当前装弹量
    public int backup;      // 后备弹夹
    public int reload_time;   // 装弹时间

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(damage);
            stream.SendNext(bulletSpeed);
            stream.SendNext(shootSpeed);
            stream.SendNext(bulletNum);
            stream.SendNext(currentBulletNum);
            stream.SendNext(backup);
            stream.SendNext(reload_time);
        }
        else
        {
            damage = (float)stream.ReceiveNext();
            bulletSpeed = (float)stream.ReceiveNext();
            shootSpeed = (int)stream.ReceiveNext();
            bulletNum = (int)stream.ReceiveNext();
            currentBulletNum = (int)stream.ReceiveNext();
            backup = (int)stream.ReceiveNext();
            reload_time = (int)stream.ReceiveNext();
        }
    }

}
