﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.MyCompany.MyGame;
using Photon.Pun;

public class StoreManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject localPlayer;
    public GameObject autoWeaponsDropDown;
    public GameObject snipeRifleDropDown;

    private bool change=false;
    private int code;
    //添加其他类别
    void Start()
    {

    }
    public void Purchase()
    {
        if (autoWeaponsDropDown.GetComponent<Dropdown>().value != 0)
        {

            //购买自动步枪武器编号，武器列表的第零位为value-1
            Debug.Log(autoWeaponsDropDown.GetComponent<Dropdown>().value - 1); 
            code = autoWeaponsDropDown.GetComponent<Dropdown>().value - 1;
            if (WeaponManager.allWeapon[code].Value > localPlayer.GetComponent<CharacterAttr>().money)
                return;
            change = true;
            localPlayer.GetComponent<CharacterAttr>().money -= WeaponManager.allWeapon[code].Value;
            localPlayer.GetComponent<WeaponManager>().addWeapon(code);
            autoWeaponsDropDown.GetComponent<Dropdown>().value = 0;
            snipeRifleDropDown.GetComponent<Dropdown>().value = 0;
        }
        //else if (snipeRifleDropDown.GetComponent<Dropdown>().value != 0)
        //{
        //    //购买狙击枪武器编号，武器列表中第一位狙击枪为automax + value
        //    Debug.Log(autoWeaponsDropDown.GetComponent<Dropdown>().options.Capacity - 1 + snipeRifleDropDown.GetComponent<Dropdown>().value);
        //    code = autoWeaponsDropDown.GetComponent<Dropdown>().options.Capacity - 1 + snipeRifleDropDown.GetComponent<Dropdown>().value - 1;
        //    localPlayer.GetComponent<WeaponManager>().addWeapon(code);
        //    change = true;
        //    autoWeaponsDropDown.GetComponent<Dropdown>().value = 0;
        //    snipeRifleDropDown.GetComponent<Dropdown>().value = 0;
        //}
        //其他类型武器
    }
    public void Back()
    {
        Cursor.visible = false;
        this.gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
            stream.SendNext(change);
            if (change)
            {
                stream.SendNext(code);
                change = false;
            }
        }
        else
        {
            if((bool)stream.ReceiveNext())
            {
                localPlayer.GetComponent<WeaponManager>().addWeapon((int)stream.ReceiveNext());
            }
        }
    }
}