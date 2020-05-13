﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public GameObject hp;
    public GameObject bullet;
    public WeaponManager wm;
    public CharacterAttr ca;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ca.gameObject != PlayerManager.localPlayer)
            this.gameObject.SetActive(false);
        hp.GetComponent<Text>().text = ca.health.ToString();
        bullet.GetComponent<Text>().text = wm.current_WeaponAttr.currentBulletNum.ToString() + "/" + wm.current_WeaponAttr.backup.ToString();
    }
}
